using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Services.Base;

namespace RealEstateApp.Infraestructure.Identity.Services.ExternalUsers
{

    public sealed class OperationalAccountWebApp 
        : BaseAccountUser,
        IOperationalAccountWebApp
    {
        private readonly IFileManager _fileManager;
        public OperationalAccountWebApp(
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            IUserSession userSession,
            IFileManager fileManager
           
            ) 
            : base(userManager, signInManager, userSession)
        {
            _fileManager = fileManager;   
        }
        #region operation create and send email confirmation
        public Task<UserResponseDto> CreateExternalAsync(RegisterExternalUsersDto registerUserDto)
        {
          
        }
        #endregion

        #region operation update data external user -> agent
        public Task<EditResponseDto> UpdateAgentAsync(EditAgentUserDto editAgent)
        {
            throw new NotImplementedException();
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
    
        //public Task<IReadOnlyCollection<CustomerConsultAgentDto>> GetAgentByConsultCustomerAsync()
        //{
        //   var results =  //comentado de momento -> 
        //}

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

        #endregion
    }

}
