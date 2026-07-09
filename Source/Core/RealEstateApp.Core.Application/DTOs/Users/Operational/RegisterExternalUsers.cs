using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.Operational
{
    public sealed class RegisterExternalUsers : RegisterUserDto
    {
        public required string Origin { get; set; }
        public required string PhoneNumber { get; set; }
        public required string ProfileImg {  get; set; }

    }
}
