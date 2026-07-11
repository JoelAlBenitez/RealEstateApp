using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.Operational
{
    public sealed class EditAgentUserDto : EditUserDto
    {
        public required string PhoneNumber { get; set; }
        public required string ProfileImg {  get; set; }
        public string? ProfileImgUrl { get; set; }
        public required bool ChangePorfileImg { get; set; }


    }
}
