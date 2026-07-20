using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.Contracts.Users.Validation;
using RealEstateApp.Core.Application.DTOs.Api.Agents;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Services.Base;

namespace RealEstateApp.Infraestructure.Identity.Services.InternalUsers
{
    public sealed class OperationalAccountWebApi :
        BaseAccountUser,
        IOperationalAccountWebApi
    {

        private readonly IServicesValidateUsers _servicesValidateUsers;


        public OperationalAccountWebApi(
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            IUserSession userSession,
            IServicesValidateUsers servicesValidateUsers
            )
            : base(userManager, 
                  signInManager, userSession)
        {
            _servicesValidateUsers = servicesValidateUsers;
        }


        #region method operational
        public async Task<UserResponseDto> CreateInternalUserAsync(
            RegisterInternalUsersDto register)
        {
            var response = new UserResponseDto
            {
                Errors = new List<string>(),
                Roles = null!,
                HasError = false
            };
            var user = await _userManager.FindByIdAsync(_userSession.GetIdCurrentUser());
            if (user == null) { 
               
                response.HasError = true;
                response.Errors.Add("Su usuario no se encuentra habilitado para realizar esta operación.");
                return response;
            }
            var rolesCurrentUser = await _userManager.GetRolesAsync(user);
            if (!rolesCurrentUser.Contains(Roles.Administrador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("No cuenta con los privilegios necesarios para realizar esta operación.");
                return response;
            }
            var validate = await _servicesValidateUsers.CreateInternalValidateUserAsync(register, response);
            if (validate != null && validate.HasError) return validate;

            var userC = new AppUsers
            {
                Name = register.Name,
                BlockedEmailSending = null,
                LastName = register.LastName,
                UserName = register.NameUser,
                Email = register.Email,
                ProfileImg = "NA",
                IsActive  = true,
                EmailConfirmed = true,
                IDCard = register.IDCard,
                CreateAt = DateTimeOffset.UtcNow
            };;

            var create = await _userManager.CreateAsync(userC, register.Password);
            if (!create.Succeeded)
            {
                response.HasError = true;
                response.Errors.Add("La solicitud no pudo ser procesada. Intente nuevamente más tarde.");
                return response;
            }
            var rol = register.TypeUser == (int)Roles.Desarrollador
                ? Roles.Desarrollador.ToString()
                : Roles.Administrador.ToString();
            var list = new List<string>();
            list.Add(rol);
            var roles = await _userManager.AddToRolesAsync(userC,list);
            return response;
        }

        public async Task<EditResponseDto> UpdateInternalUserAsync(EditInternalUserDto edit)
        {
            var response = new EditResponseDto
            {
                Errors = new List<string>(),
                HasError = false   
            };
            var validate = await _servicesValidateUsers
                .UpdateInternalValidateUserAsync(edit, response);
            if (validate != null && validate.HasError) return validate;
            var user = await _userManager.FindByIdAsync(edit.Id);
            if(user == null)
            {
                response.HasError = true;
                response.Errors.Add("Ha ocurrido un error al seleccionar el usuario.");
                return response;
            }
            user.Email = edit.Email;
            user.IDCard = edit.IdCard;
            user.Name = edit.Name;
            user.UserName = edit.UserName;
            user.LastName = edit.LastName;
            if (!string.IsNullOrWhiteSpace(edit.NewPassword)) {
                var changePassword = await _userManager.ChangePasswordAsync(user, user.PasswordHash!,edit.NewPassword);
                if (!changePassword.Succeeded)
                {
                    response.HasError = true;
                    response.Errors.Add("Ha ocurrido un error inesperado al editar el usuario.");
                    return response;
                }
                return response;
            }
            
            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
            {
                response.HasError = true;
                response.Errors.Add("Ha ocurrido un error inesperado al editar el usuario.");
                return response;
            }

            return response;
        }

        #endregion
        #region get methods

        public async Task<IReadOnlyCollection<AgentApiDto>> GetAllAgentsForApiAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());
            if (users == null) return [];
            var select = users.Select(u => new AgentApiDto
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email ?? string.Empty,
                Phone = u.PhoneNumber ?? string.Empty,
                NumberOfProperties = 0,
                IsActive = u.IsActive
            }).ToList();
            return select;
        }

        public async Task<AgentApiDto?> GetAgentByIdForApiAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;
            var isAgent = await _userManager.IsInRoleAsync(user, Roles.Agente.ToString());
            if (!isAgent) return null;
            return new AgentApiDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                NumberOfProperties = 0,
                IsActive = user.IsActive
            };
        }

        public async Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAgentPendientConfirmAccount()
        {
            var result = await _userManager.Users
                 .AsNoTracking()
                 .Where(u => !u.EmailConfirmed && !u.IsActive).ToListAsync();
            if (result == null) return [];
            var select = result.Select(s => new AdminConsultAgentDto
            {
                Email = s.Email!,
                Id = s.Id,
                State = s.IsActive,
                LastName = s.LastName,
                Name = s.Name,
                Properties = 0
            }).ToList();
            return select;
        }

        public async Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAllAgentesByConsultAdmin()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());
            if (users == null) return [];
            var select = users.Select(s => new AdminConsultAgentDto
            {
                Id = s.Id,
                Name = s.Name,
                LastName = s.LastName,
                Email = s.Email!,
                State = s.IsActive,
                Properties = 0
            }).ToList();
            return select;
        }

        public async Task<IReadOnlyCollection<GetInternalUserDto>> GetAllInternalUsersByRol(Roles roles)
        {
            if (roles != Roles.Desarrollador && roles != Roles.Administrador) return [];
            var result = await _userManager.GetUsersInRoleAsync(roles.ToString());
            if (result == null) return [];
            var select = result.Select(s => new GetInternalUserDto
            {
                IDCard = s.IDCard!,
                State = s.IsActive,
                Name = s.Name,
                Id = s.Id,
                LastName = s.LastName,
                Email   = s.Email!,
                UserName = s.UserName!
            }).ToList();
            return select;
        }

        public async Task<int> GetUserAgentActiverOrInactive(bool isActive = true)
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Agente.ToString());
            return users.Count(u => u.IsActive == isActive);
        }

        public async Task<int> GetUserClientAciveOrInactive(bool isActive = true)
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Cliente.ToString());
            return users.Count(u => u.IsActive == isActive);
        }

        public async Task<int> GetUserDevelopersActiveOrInactive(bool isActive = true)
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Desarrollador.ToString());
            return users.Count(u => u.IsActive == isActive);
        }

        public async Task<List<string>> GetRolesConfirmRol(string IdUser)
        {
            var user = await _userManager.FindByIdAsync(IdUser);
            if(user == null) { return new List<string>(); }
            var roles = await _userManager.GetRolesAsync(user);
            return (List<string>)roles;
        }



        #endregion

    }
}
