using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
namespace RealEstateApp.Core.Application.Contracts.Users.ExternalUsers
{
    public interface IOperationalAccountWebApp
    {
        Task<IReadOnlyCollection<ClientDto>> GetClientAllAsync(List<string> Ids);
        Task<IReadOnlyCollection<ConsultAgentDto>> GetAgentAllAsync(List<string> Ids);
        //Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentByConsultCustomerAsync();
        Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentAllViewHomeByCustomer();
        Task<CustomerConsultAgentDto> GetAgentByConsultCustomerByUserNameAgent(string userName);
        Task<ConsultAgentDto> GetConsultAgentById(string id);
        Task<UserResponseDto> CreateExternalAsync(RegisterExternalUsersDto registerUserDto);
        Task<EditResponseDto> UpdateAgentAsync(EditAgentUserDto editAgent);
    }
}

