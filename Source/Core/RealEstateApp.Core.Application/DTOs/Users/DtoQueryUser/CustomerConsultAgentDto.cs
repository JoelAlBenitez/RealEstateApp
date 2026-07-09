using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser
{
    public sealed class CustomerConsultAgentDto : BaseGetUserDto
    {
        public required string ProfileImgAgent {  get; set; }
    }
}
