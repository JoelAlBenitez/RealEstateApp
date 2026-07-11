using AutoMapper;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Application.ViewsModel.MessageAtC;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Message
{
    public sealed class MessageDtoToViewModelAndReverse : Profile
    {
        public MessageDtoToViewModelAndReverse()
        {
            CreateMap<MessageAtCDto, MessageAtCViewModel>().ReverseMap();
            CreateMap<SaveMessageAtCDto, SendMessageAtCViewModel>().ReverseMap();
        }
    }
}
