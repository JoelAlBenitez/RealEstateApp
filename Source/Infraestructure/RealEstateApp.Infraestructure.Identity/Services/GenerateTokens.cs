using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Services.Interfaces;
using System.Text;

namespace RealEstateApp.Infraestructure.Identity.Services
{
    public class GenerateTokens : IGenerateTokens
    {

        private readonly UserManager<AppUsers> _userManager;

        public GenerateTokens(UserManager<AppUsers> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenConfirmEmail(AppUsers users, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(users);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "Account/ConfirmAccountEmail";
            var completeUrl = new Uri(string.Concat(origin, "/", route));
            var verifyemail = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", users.Id);
            verifyemail = QueryHelpers.AddQueryString(verifyemail.ToString(), "token", token);
            return verifyemail;

        }

        public async Task<string> GenerateTokenResetPassword(AppUsers users, string origin)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(users);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "Account/ResetPassword";
            var completeUrl = new Uri(string.Concat(origin, "/", route));
            var resetPassword = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", users.Id);
            resetPassword = QueryHelpers.AddQueryString(resetPassword.ToString(), "token", token);
            return resetPassword;
        }
    }
}
