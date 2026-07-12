using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.Contracts.Users.Validation;
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

        public Task<UserResponseDto> CreateInternalUserAsync(RegisterInternalUsersDto register)
        {
            throw new NotImplementedException();
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
                IDCard = s.IDCard,
                State = s.IsActive,
                Name = s.Name,
                Id = s.Id,
                LastName = s.LastName,
                Email   = s.Email!,
                UserName = s.UserName!
            }).ToList();
            return select;
        }

        public Task<UserResponseDto> UpdateInternalUserAsync(EditInternalUserDto edit)
        {
            throw new NotImplementedException();
        }
    }
}
