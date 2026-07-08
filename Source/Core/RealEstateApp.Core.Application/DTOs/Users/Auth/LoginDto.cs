namespace RealEstateApp.Core.Application.DTOs.Users.Auth
{
    public sealed record  LoginDto
    {
        public required string EmailOrNameUser { get; set; }
        public required string Password { get; set; }
    }
}
