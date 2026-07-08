namespace RealEstateApp.Core.Application.DTOs.Users.Password
{
    public sealed record ForgoutPasswordDto
    {
        public required string UserName { get; set; }
        public required string Origin { get; set; }
    }
}
