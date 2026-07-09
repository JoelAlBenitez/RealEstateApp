using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.Operational
{
    public sealed class EditInernalUserDto  : EditUserDto
    {
        public required string Email { get; set; }
        public required string IdCard { get; set; }
        public required string UserName { get; set; }
        public  string? NewPassword { get; set; }
        public  string? ConfirmNewPassword { get; set; }
    }
}
