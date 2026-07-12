using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Core.Application.Contracts.Users.InternalUsers
{
    public interface IOperationalAccountWebApi
    {
        Task<UserResponseDto> CreateInternalUserAsync(RegisterInternalUsersDto register);
        Task<UserResponseDto> UpdateInternalUserAsync(EditInternalUserDto edit);
        Task<IReadOnlyCollection<GetInternalUserDto>> GetAllInternalUsersByRol(Roles roles);
        Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAllAgentesByConsultAdmin();



    }
}
