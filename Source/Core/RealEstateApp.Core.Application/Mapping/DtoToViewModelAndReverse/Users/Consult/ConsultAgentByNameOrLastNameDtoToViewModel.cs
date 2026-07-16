using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Consult
{
    public sealed class ConsultAgentByNameOrLastNameDtoToViewModel: Profile
    {
        public ConsultAgentByNameOrLastNameDtoToViewModel()
        {
            CreateMap<ConsultAgentByNameOrLastNameDto, AgentConsultByNameOrLastNameViewModel>();
        }
    }
}
