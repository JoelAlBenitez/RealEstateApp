namespace RealEstateApp.Core.Application.DTOs.Users.Auth
{
    public sealed class LoginApiDtoResponse
    {
        public required string Token { get; set; }
        public required string UserName { get; set; }
        public required List<string> Roles {  get; set; }
        public DateTime? Expiration { get; set; }
        public required bool HasError { get; set; }
        public bool Forbidden { get; set; }
        public required List<string> Errors { get; set; }

    }
}
