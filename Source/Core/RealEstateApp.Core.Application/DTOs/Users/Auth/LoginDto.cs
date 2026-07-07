namespace RealEstateApp.Core.Application.DTOs.Users.Auth
{
    public sealed record  LoginDto
    {
        public required string CorreoOrUserName { get; set; }
        public required string Password { get; set; }
    }
}
