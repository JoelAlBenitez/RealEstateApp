using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Infraestructure.Identity.Services
{
    public sealed class OperationalAccountWebApp : IOperationalAccountWebApp
    {



        #region agents users consult
        public Task<IReadOnlyCollection<ConsultAgentDto>> GetAgentAllAsync(List<string> Ids)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentAllViewHomeByCustomer()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<AdminConsultAgentDto>> GetAgentByConsultAdminAll()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentByConsultCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerConsultAgentDto> GetAgentByConsultCustomerByUserNameAgent(string userName)
        {
            throw new NotImplementedException();
        }

    

        public Task<ConsultAgentDto> GetConsultAgentById(string id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region clients users consults
        public Task<IReadOnlyCollection<ClientDto>> GetClientAllAsync(List<string> Ids)
        {
            throw new NotImplementedException();
        }
        public Task<IReadOnlyCollection<GetInternalUserDto>> GetInternalUserGetAll(Roles roles)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
