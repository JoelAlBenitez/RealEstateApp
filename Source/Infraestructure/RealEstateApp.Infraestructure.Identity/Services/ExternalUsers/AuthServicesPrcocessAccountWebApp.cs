using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Services.ExternalUsers
{
    public sealed class AuthServicesPrcocessAccountWebApp : IAuthProcesssAccountWebApp
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
      
        //por implementar
        public AuthServicesPrcocessAccountWebApp(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<string> ConfirmAccountByEmailAsync(string token, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDto> ForgoutPasswordAsync(ForgoutPasswordDto forgoutPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDto> LoginUser(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDto> ResendActivationEmailAsync(ResendActivationEmailDto resendActivationEmailDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }
    }
}
