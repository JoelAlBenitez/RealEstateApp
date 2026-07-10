using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Contracts.Users;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Services
{
    public class AuthServicesPrcocessAccountWebApp : IAuthProcesssAccountWebApp
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;

        public AuthServicesPrcocessAccountWebApp(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<UserResponseDto> ForgoutPasswordAsync(ForgoutPasswordDto forgoutPasswordDto)
        {
            throw new NotImplementedException();
        }
      
        public Task<UserResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }
    }
}
