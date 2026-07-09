namespace RealEstateApp.Core.Application.DTOs.Users.Password
{
    public sealed class  ResetPasswordDto
    {

        public required string Id { get; set; }
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmNewPassword { get; set; }
    }
}
