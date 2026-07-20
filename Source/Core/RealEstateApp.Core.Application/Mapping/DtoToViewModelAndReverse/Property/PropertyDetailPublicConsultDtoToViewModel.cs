using AutoMapper;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Property
{
    public sealed class PropertyDetailPublicConsultDtoToViewModel : Profile
    {
        public PropertyDetailPublicConsultDtoToViewModel()
        {
            CreateMap<PropertyDto, PropertyDetailPublicConsult>()
                .ForMember(opt => opt.Id, des => des.MapFrom(src => src.Id))
                .ForMember(opt => opt.Price, des => des.MapFrom(src => src.Price))
                .ForMember(opt => opt.Code, des => des.MapFrom(src => src.Code))
                .ForMember(opt => opt.Size, des => des.MapFrom(src => src.Size))
                .ForMember(opt => opt.Bathrooms, des => des.MapFrom(src => src.Bathrooms))
                .ForMember(opt => opt.Bedrooms, des => des.MapFrom(src => src.Bedrooms))
                .ForMember(opt => opt.AgentEmail, des => des.MapFrom(src => src.AgentEmail))
                .ForMember(opt => opt.AgentName, des => des.MapFrom(src => src.AgentName))
                .ForMember(opt => opt.AgentImg, des => des.MapFrom(src => src.AgentPhotoUrl))
                .ForMember(opt => opt.AgentPhone, des => des.MapFrom(src => src.AgentPhone))
                .ForMember(opt => opt.Description, des => des.MapFrom(src => src.Description))
                .ForMember(opt => opt.ImgUrls, des => des.MapFrom(src => src.Images.Select(a => a.Url)))
                .ForMember(opt => opt.Improvents, des => des.MapFrom(src => src.Impro)) 
        }
    }
}
