using RealEstateApp.Infraestructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstateApp.Infraestructure.Identity.Interfaces
{
    public interface IGenerateTokens
    {
        Task<string> GenerateTokenResetPassword(AppUsers users, string origin);
        Task<string> GenerateTokenConfirmEmail(AppUsers users, string origin);
        Task<JwtSecurityToken> GenerateJwtToken(AppUsers user);
    }
}
