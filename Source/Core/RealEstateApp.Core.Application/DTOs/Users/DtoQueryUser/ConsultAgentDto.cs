using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser
{
    public sealed class ConsultAgentDto : BaseGetUserDto
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string ProfileImgAgent { get; set; }
    }
}
