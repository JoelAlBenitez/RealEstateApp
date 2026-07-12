using AutoMapper;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.DTOs.MessageAtC;

namespace RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Message
{
    public sealed class MessageEntityToDtoAndReverse : Profile
    {
        public MessageEntityToDtoAndReverse()
        {
            CreateMap<Domain.Entities.Message, MessageAtCDto>().ReverseMap();
            CreateMap<Domain.Entities.Message, SaveMessageAtCDto>().ReverseMap();
        }
    }
}
