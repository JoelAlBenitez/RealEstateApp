using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.DTOs.Users.Response;

using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Core.Application.Contracts.Users
{
    public interface IOperationalAccountWebApp
    {
        Task<IReadOnlyCollection<ClientDto>> GetClientAllAsync(List<string> Ids);
        Task<IReadOnlyCollection<ConsultAgentDto>> GetAgentAllAsync(List<string> Ids);
        Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentByConsultCustomerAsync();


        Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAgentByConsultAdminAll();
        Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentAllViewHomeByCustomer();
        Task<CustomerConsultAgentDto> GetAgentByConsultCustomerByUserNameAgent(string userName);
        Task<ConsultAgentDto> GetConsultAgentById(string id);
        Task<UserResponseDto> ForgoutPasswordAsync(ForgoutPasswordDto forgoutPasswordDto);
        Task<UserResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        Task<IReadOnlyCollection<GetInternalUserDto>> GetInternalUserGetAll(Roles roles);

    }
}