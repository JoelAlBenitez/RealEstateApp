using Azure;
using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Contracts.EmailServices;
using RealEstateApp.Core.Application.Contracts.Users.Base;
using RealEstateApp.Core.Application.DTOs.Message;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Services.Interfaces;
using System.Text.RegularExpressions;

namespace RealEstateApp.Infraestructure.Identity.Services.Base
{
    public sealed class BaseAccountServices : IBaseAccountUser
    {

        private readonly UserManager<AppUsers> _userManager;
        private readonly IEmailService _emailService;
        private readonly IGenerateTokens _generateTokens;

        public BaseAccountServices(
            UserManager<AppUsers> userManager,
            IEmailService emailService,
            IGenerateTokens generateTokens
            
            )
        {
            _userManager = userManager;
            _emailService = emailService;
            _generateTokens = generateTokens;
        }

        public async Task<UserResponseDto> ChangeStateAsync(AlterStateUserDto alterStateUserDto,
            string IdUserCurrent)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Errors = new List<string>(),
                Roles = null!,
            };
            var user = await _userManager.FindByIdAsync(alterStateUserDto.Id);
            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add("El usuario que ha intentando operar no pudo ser encontrado, favor intente de nuevo mas tarde");
                return response;
            }
            var existUser = await _userManager.FindByIdAsync(IdUserCurrent);
            if (existUser != null)
            {
                bool IsAdmin = await _userManager.IsInRoleAsync(existUser, Roles.Administrador.ToString());
                user.IsActive = alterStateUserDto.State;
                await _userManager.UpdateAsync(user);
                return response;
            }
            response.HasError = true;
            response.Errors.Add("No posee permisos para realizar esta accion");
            return response;
        }


        public async Task<UserResponseDto> DeleteAsync(string IdUser)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Errors = new List<string>(),
                Roles = null!,

            };
            var user = await _userManager.FindByIdAsync(IdUser);
            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add("El usuario indicado no se encuentra disponible para operaciones, favor intente de nuevo mas tarde.");
                return response;
            }
            var result = await _userManager.DeleteAsync(user);
            if(!result.Succeeded)
            {
                response.HasError = true;
                response.Errors.Add("El usuario no pudo ser eliminado favor intente de nuevo mas tarde");
                return response;
            }
            return response;
        }

        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }

        #region create
        public async Task<UserResponseDto> CreateExternalAsync(RegisterExternalUsers registerUserDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Errors = new List<string>(),
                Roles = new List<string>()
            };

            var existUser = await ExistUserByEmailOrUserName(response, registerUserDto.Email, registerUserDto.NameUser);
            if (existUser != null && response.HasError) return existUser;

            var validateFields = ValidateFieldsCreateExternalUser(response, registerUserDto);
            if (validateFields != null && validateFields.HasError) return validateFields;

            var validatePassword = ValidatePassword(registerUserDto.Password, registerUserDto.ConfirmPassword, response);
            if (validatePassword != null && validatePassword.HasError) return validatePassword;
            if(registerUserDto.TypeUser == (int) Roles.Cliente && string.IsNullOrWhiteSpace(registerUserDto.Origin))
            {
                response.HasError = true;
                response.Errors.Add("La solicitud no puede ser procesada en este momento favor intente de nuevo mas tarde");
                return response;
            }

            var user = new AppUsers
            {
                Name = registerUserDto.Name,
                LastName = registerUserDto.LastName,
                UserName = registerUserDto.NameUser,
                Email = registerUserDto.Email,
                IDCard = "NA",
                PhoneNumber = registerUserDto.PhoneNumber,
                ProfileImg = registerUserDto.ProfileImg,
                IsActive = false,
                EmailConfirmed = false,
                BlockedEmailSending = null,
                CreateAt = DateTimeOffset.UtcNow           
            };

            var success = await _userManager.CreateAsync(user, registerUserDto.Password);

            if(success.Succeeded)
            {
                if(registerUserDto.TypeUser == (int) Roles.Agente)
                    await _userManager.AddToRoleAsync(user, Roles.Agente.ToString());
                if (registerUserDto.TypeUser == (int)Roles.Cliente)
                    await _userManager.AddToRoleAsync(user, Roles.Cliente.ToString());
                if(registerUserDto.TypeUser == (int)Roles.Cliente)
                {
                    var send = await SendEmailConfirm(user, registerUserDto.Origin, response);
                    if (send != null && send.HasError) return send;
                }

            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(success.Errors.Select(s => s.Description).ToList());
                return response;
            }

           return response;
        }

        public Task<UserResponseDto> CreateInternalAsync(RegisterInternalUsersDto registerUserDto, bool isApi)
        {
            throw new NotImplementedException();
        }

        #endregion;
       
        public async Task<BaseGetUserDto> GetById(string IdUser)
        {
            var user = await _userManager.FindByIdAsync(IdUser);
            return new BaseGetUserDto
            {
                Id = user!.Id,
                Name = user.Name,
                LastName = user.LastName,
            };
        }

        #region update
        public async Task<EditResponseDto> UpdateAgentAsync(EditAgentUserDto editAgent)
        {
            throw new NotImplementedException();
        }

        public Task<EditResponseDto> UpdateInternalAsync(EditInernalUserDto edit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region private methods
        private async Task<UserResponseDto> SendEmailConfirm(AppUsers user, string Origin, UserResponseDto response)
        {
            string verificationEmail = await _generateTokens.GenerateTokenConfirmEmail(user, Origin);
            var send = await _emailService.SendEmailAsync(new MessageDto
            {
                To = user.Email!,
                Subject = "Real State App",
                Body = "<div style = 'background:#4f46e5;color:white;padding:20px;text-align:center;font-size:20px;' >" +
                "<h2> Link Up </h2> " +
                "<p> Bienvenido a  Real State App  para continuar con el proceso y disfrutar de nuestro Airbnb favor verifique su cuenta </p>" +
                $"<p style = 'color:#fff;' >{verificationEmail}<p>" +
                "<p><b>Nota:</b> Si usted no ha realizado esta solicitud ignore este mensaje.</p>" +
                " </div>",
            });
            if (!send)
            {
                response.HasError = true;
                response.Errors.Add("Su cuenta no pudo ser confirmada de momento, favor intente de nuevo mas tarde o solicite otro correo de confirmacion.");
                return response;
            }
            user.BlockedEmailSending = DateTimeOffset.UtcNow;
            await _userManager.UpdateAsync(user);
            return response;
        }

        private UserResponseDto ValidateFieldsCreateExternalUser(UserResponseDto userResponseDto,
            RegisterExternalUsers registerExternalUsers
            )
        {
            if (string.IsNullOrWhiteSpace(registerExternalUsers.Email) ||
                string.IsNullOrWhiteSpace(registerExternalUsers.PhoneNumber) ||
                string.IsNullOrWhiteSpace(registerExternalUsers.NameUser) ||
                string.IsNullOrWhiteSpace(registerExternalUsers.Name) ||
                string.IsNullOrWhiteSpace(registerExternalUsers.LastName) ||
                string.IsNullOrWhiteSpace(registerExternalUsers.Password) ||
                string.IsNullOrWhiteSpace(registerExternalUsers.ConfirmPassword)
                
                )
            {

                userResponseDto.HasError = true;
                userResponseDto.Errors.Add("Debe rellenar todos los campos del usuario");
                return userResponseDto;
            }

            if (string.IsNullOrWhiteSpace(registerExternalUsers.ProfileImg))
                userResponseDto.Errors.Add("La imagen ingresa no pudo se procesada verifique" +
                    " si la misma tiene un formato valido (JPG, PNG, JPEG) y no mauor a 5 mb ");
            
            if(registerExternalUsers.TypeUser != (int)Roles.Agente
                || registerExternalUsers.TypeUser != (int)Roles.Cliente)
                userResponseDto.Errors.Add("El tipo de usuario especificado no es valido");
            
            if (!Regex.IsMatch(registerExternalUsers.PhoneNumber, "^(809|829|849)-\\d{3}-\\d{4}$"))
                userResponseDto.Errors.Add("Debe ingresar un número telefónico válido de República Dominicana.");
            if (userResponseDto.Errors.Any())
            {
                userResponseDto.HasError = true;
                return userResponseDto;
            }
            return userResponseDto;
        }

        private UserResponseDto ValidatePassword(string password, string confirmPasssword, UserResponseDto userResponseDto)
        {
            if(password == confirmPasssword)
            {
                userResponseDto.HasError = true;
                userResponseDto.Errors.Add("Las contraseñas deben coincidir");
                return userResponseDto;
            }
            return userResponseDto;
        }
        private async Task<UserResponseDto> ExistUserByEmailOrUserName(UserResponseDto response, string Email, string NameUser)
        {
            var existByEmail = await _userManager.FindByEmailAsync(Email);
            if (existByEmail != null)
            {
                response.HasError = true;
                response.Errors.Add("El email ingresado ya encuentra asociado a una cuenta, favor intente con otro.");
                return response;
            }
            if (existByEmail!.UserName == NameUser)
            {
                response.HasError = true;
                response.Errors.Add("El nombre de usuario ya se encuentra asociado a una cuenta.");
                return response;
            }
            return response;
        }

      


        #endregion
    }
}
