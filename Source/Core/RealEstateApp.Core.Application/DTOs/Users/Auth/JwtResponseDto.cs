namespace RealEstateApp.Core.Application.DTOs.Users.Auth
{
    public sealed class JwtResponseDto
    {
        public required bool HasError { get; set; }
        public required string Errors { get; set; }
    }
}
