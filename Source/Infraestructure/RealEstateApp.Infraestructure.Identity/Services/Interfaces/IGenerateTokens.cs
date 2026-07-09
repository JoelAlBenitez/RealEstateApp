using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Services.Interfaces
{
    public interface IGenerateTokens
    {
        Task<string> GenerateTokenResetPassword(AppUsers users, string origin);
        Task<string> GenerateTokenConfirmEmail(AppUsers users, string origin);
    }
}
