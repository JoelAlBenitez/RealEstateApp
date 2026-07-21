using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using System.Security.Claims;

namespace RealEstateApp.Presentation.Api.Helpers
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetIdCurrentUser()
        {
            // El token JWT emite el Id del usuario en el claim "uid";
            // NameIdentifier se mantiene como respaldo.
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid")
                ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        public List<string> GetRolesCurrentUser()
        {
            // El token JWT emite los roles en el claim "roles" (ver GenerateTokens).
            return _httpContextAccessor.HttpContext?.User?
                .FindAll("roles")
                .Select(r => r.Value).ToList() ?? new List<string>();

        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name)!;
        }
    }
}
