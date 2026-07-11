namespace RealEstateApp.Core.Application.DTOs.Users.Auth
{
    public sealed class ResendActivationEmailDto
    {
        public required string UserName { get; set; }
        public required string Origin { get; set; }
    }
}
