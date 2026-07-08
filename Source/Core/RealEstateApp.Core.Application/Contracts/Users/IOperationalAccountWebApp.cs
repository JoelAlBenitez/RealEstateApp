using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Core.Application.Contracts.Users
{
    public interface IOperationalAccountWebApp
    {
        Task<IReadOnlyCollection<ClientDto>> GetClientAllAsync(List<string> Ids);
        Task<IReadOnlyCollection<ConsultAgentDto>> GetAgentAllAsync(List<string> Ids);
        Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentByConsultCustomerAsync();
        Task<IReadOnlyCollection<GetInternalUserDto>> GetInternalUserGetAll();
        Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAgentByConsultAdminAll();
        Task<UserResponseDto> ForgoutPasswordAsync(ForgoutPasswordDto forgoutPasswordDto);
        Task<UserResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
