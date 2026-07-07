using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser
{
    public sealed record CustomerInquiryAgentDto : BaseGetUserDto
    {
        public required string ProfileImgAgent {  get; set; }
    }
}
