using RealEstateApp.Core.Application.Contracts.Users.Base;
using RealEstateApp.Core.Application.DTOs.Api.Agents;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Core.Application.Contracts.Users.InternalUsers
{
    public interface IOperationalAccountWebApi : IBaseAccountUser
    {
        Task<IReadOnlyCollection<AgentApiDto>> GetAllAgentsForApiAsync();
        Task<AgentApiDto?> GetAgentByIdForApiAsync(string id);
        Task<UserResponseDto> CreateInternalUserAsync(RegisterInternalUsersDto register);
        Task<EditResponseDto> UpdateInternalUserAsync(EditInternalUserDto edit);
        Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAgentPendientConfirmAccount();
        Task<int> GetUserAgentActiverOrInactive(bool isActive = true);
        Task<int> GetUserDevelopersActiveOrInactive(bool isActive = true);
        Task<int> GetUserClientAciveOrInactive(bool isActive = true);
        Task<IReadOnlyCollection<GetInternalUserDto>> GetAllInternalUsersByRol(Roles roles);
        Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAllAgentesByConsultAdmin();



    }
}
