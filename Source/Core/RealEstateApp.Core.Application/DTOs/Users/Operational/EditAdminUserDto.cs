using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.Operational
{
    public sealed record EditAdminUserDto  : EditUserDto
    {
        public required string Email { get; set; }
        public required string IdCard { get; set; }
        public required string UserName { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmNewPassword { get; set; }
    }
}
