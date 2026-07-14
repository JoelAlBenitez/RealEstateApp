using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Core.Domain.Settings.JWT;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateApp.Infraestructure.Identity.Services.GenerateTokens
{
    public class GenerateTokens : IGenerateTokens
    {

        private readonly UserManager<AppUsers> _userManager;
        private readonly JwtSettings _jwtSettings;

        public GenerateTokens(
            UserManager<AppUsers> userManager,
            JwtSettings jwtSettings            
            )
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<JwtSecurityToken> GenerateJwtToken(AppUsers user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClaims = new List<Claim>();
            foreach (var role in roles)
            {
                rolesClaims.Add(new Claim("roles", role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid",user.Id ?? "")
            }.Union(userClaims).Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
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
