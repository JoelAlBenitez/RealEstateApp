using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Interfaces
{
    public interface IGenerateTokens
    {
        Task<string> GenerateTokenResetPassword(AppUsers users, string origin);
        Task<string> GenerateTokenConfirmEmail(AppUsers users, string origin);
    }
}
