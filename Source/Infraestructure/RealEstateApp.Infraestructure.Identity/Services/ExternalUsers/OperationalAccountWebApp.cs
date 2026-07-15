using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RealEstateApp.Core.Application.Contracts.EmailServices;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.Contracts.Users.Validation;
using RealEstateApp.Core.Application.DTOs.Message;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Context;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Interfaces;
using RealEstateApp.Infraestructure.Identity.Services.Base;

namespace RealEstateApp.Infraestructure.Identity.Services.ExternalUsers
{

    public sealed class OperationalAccountWebApp 
        : BaseAccountUser,
        IOperationalAccountWebApp
    {
        private readonly IFileManager _fileManager;
        private readonly IServicesValidateUsers _servicesValidateUsers;
        private readonly IEmailService _emailService;
        private readonly IGenerateTokens _generateTokens;
        private readonly DbContextIdentityRealStateApp _context;

        public OperationalAccountWebApp(
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            IUserSession userSession,
            IFileManager fileManager,
            IServicesValidateUsers servicesValidateUsers,
            IEmailService emailService,
            IGenerateTokens generate,
            DbContextIdentityRealStateApp context
           
            ) 
            : base(userManager, signInManager, userSession)
        {
            _fileManager = fileManager;   
            _servicesValidateUsers = servicesValidateUsers;
            _emailService = emailService;
            _generateTokens = generate;
            _context = context;
        }
        #region operation create and send email confirmation
        public async Task<UserResponseDto> CreateExternalAsync(RegisterExternalUsersDto registerUserDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Errors = new List<string>(),
                Roles = new List<string>()
            };
            var validate = await _servicesValidateUsers
                .CreateExternalValidateUserAsync(registerUserDto, response);
            if (validate != null && validate.HasError) return validate;
            //user creation
            var user = new AppUsers
            {
                Name = registerUserDto.Name,
                LastName = registerUserDto.LastName,
                BlockedEmailSending = null,
                CreateAt = DateTimeOffset.UtcNow,
                UserName = registerUserDto.NameUser,
                ProfileImg = registerUserDto.ProfileImg,
                PhoneNumber = registerUserDto.PhoneNumber,
                Email = registerUserDto.Email,
                IsActive = false,
                IDCard = null
            };
            //..
            var create = await _userManager.CreateAsync(user, registerUserDto.Password);
            if (!create.Succeeded)
            {
                response.HasError = true;
                if (create.Errors.Any())
                    response.Errors.AddRange(create.Errors.Select(e => e.Description));
                else
                    response.Errors.Add("No fue posible completar el registro. Intente nuevamente más tarde.");
                return response;
            }
            var rol = registerUserDto.TypeUser == (int)Roles.Agente ?
                Roles.Agente.ToString() : Roles.Cliente.ToString();
            await _userManager.AddToRoleAsync(user, rol);

            if (rol == Roles.Cliente.ToString())
            {
                var verifiyTokens = await _generateTokens.GenerateTokenConfirmEmail(user, registerUserDto.Origin);
                var sendEmail = await _emailService.SendEmailAsync(new MessageDto
                {
                    Subject = "RealEstateApp",
                    To = user.Email,
                    Body = $"<p>¡Bienvenido a <strong>RealEstateApp</strong>!</p>" +
                    $"<p><a href='{verifiyTokens}'>Haz clic aquí para confirmar tu cuenta.</a></p>"
                });

                if (!sendEmail)
                {
                    response.HasError = true;
                    response.Errors.Add("No fue posible enviar el correo de confirmación. Intente nuevamente más tarde.");
                    return response;
                }
            }

            return response;

        }
        #endregion

        #region operation update data external user -> agent
        public async Task<EditResponseDto> UpdateAgentAsync(EditAgentUserDto editAgent)
        {
            var response = new EditResponseDto
            {
                Errors = new List<string>(),
                HasError = false

            };
            var validate = await _servicesValidateUsers.UpdateExternalValidateUserAsync(editAgent, response);
            if (validate != null && validate.HasError) return validate;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existUser = await _userManager.FindByIdAsync(editAgent.Id);
                if (existUser == null)
                {
                    response.HasError = true;
                    response.Errors.Add("La solicitud no pudo ser procesada. Intente nuevamente más tarde.");
                    return response;
                }

                string Id = "";
                if (!string.IsNullOrWhiteSpace(editAgent.ProfileImg))
                {
                    Id = editAgent.ProfileImg.Split('/')[2];
                }
                existUser.Name = editAgent.Name;
                existUser.LastName = editAgent.LastName;
                existUser.PhoneNumber = editAgent.PhoneNumber;
                if (editAgent.ChangePorfileImg)
                {
                    existUser.ProfileImg = editAgent.ProfileImg;
                }

                var updateUser = await _userManager.UpdateAsync(existUser);
                if (!updateUser.Succeeded)
                {
                    await transaction.RollbackAsync();
                    response.HasError = true;
                    response.Errors.Add("Su perfil no pudo ser modificado. Intente nuevamente más tarde.");
                    return response;
                }

                if (!await DeleteProfileImgAsync(response, Id, editAgent, transaction))
                    return response;

                await transaction.CommitAsync();
                return response;
            }
            catch (Exception)
            {

                response.HasError = true;
                response.Errors.Add("Ha ocurrido un error inesperado al procesar la solicitud. Intente nuevamente más tarde.");
                return response;
            }


        }
        #endregion

        #region agents users consult
        public async Task<IReadOnlyCollection<ConsultAgentDto>> GetAgentAllAsync(List<string> Ids)
        {
           var result = await _userManager.Users.AsNoTracking()
                .Where(u => Ids.Contains(u.Id))
                .ToListAsync();
            if (result == null)return [];
            var agentsResult = result.Select(a => new ConsultAgentDto
            {
                Id = a.Id,
                PhoneNumber  = a.PhoneNumber!,
                ProfileImgAgent = a.ProfileImg,
                Email = a.Email!,
                LastName = a.LastName,
                Name = a.Name

            }).ToList();
            return agentsResult;
        }
        public async Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentAllViewHomeByCustomer()
        {
            var result = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());
            if (result == null) return [];
            var agents = result.Select(a => new CustomerConsultAgentDto
            {
                Id = a.Id,
                Name = a.Name,
                LastName = a.LastName,
                ProfileImgAgent = a.ProfileImg
            }).ToList();
            return agents;
        }
        public async Task<CustomerConsultAgentDto> GetAgentByConsultCustomerByUserNameAgent(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            if (result == null) return null!;
            return new CustomerConsultAgentDto
            {
                Id = result!.Id,
                LastName = result.LastName,
                Name = result.Name,
                ProfileImgAgent = result.ProfileImg
                
            };
        }
        public async Task<ConsultAgentDto> GetConsultAgentById(string id)
        {
            var result  = await _userManager.FindByIdAsync(id);
            if (result == null) return null!;
            return new ConsultAgentDto
            {
                Email = result.Email!,
                Id = result.Id,
                Name = result.Name,
                LastName= result.LastName,
                PhoneNumber  = result.PhoneNumber!,
                ProfileImgAgent = result.ProfileImg
            };
        }
        #endregion

        #region clients users consults
        public async Task<IReadOnlyCollection<ClientDto>> GetClientAllAsync(List<string> Ids)
        {
            var results = await _userManager.Users.
                AsNoTracking()
                .Where(u => Ids.Contains(u.Id)).ToListAsync();
            if (results == null) return [];
            var clients = results.Select(c => new ClientDto
            {
                Id = c.Id,
                Name = c.Name,
                LastName  = c.LastName
            }).ToList();
            return clients;
        }


        #endregion

        #region private methods

        private async Task<bool> DeleteProfileImgAsync(EditResponseDto response,
            string imgId, EditAgentUserDto edit, IDbContextTransaction contextTransaction)
        {

            if (!edit.ChangePorfileImg)
                return true;

            var deleteProfile = await _fileManager.DeleteAsync("Users", imgId);
            if (deleteProfile)
                return true;

            response.HasError = true;
            response.Errors.Add("La imagen no pudo ser procesada. Intente nuevamente más tarde.");

            await contextTransaction.RollbackAsync();
            return false;
        }
        #endregion
    }

}
