namespace RealEstateApp.Core.Application.DTOs.Users.Auth
{
    public sealed class  LoginDto
    {
        public required string EmailOrNameUser { get; set; }
        public required string Password { get; set; }
    }
}
