using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Core.Application.Contracts.Users
{
    public interface IAuthProcesssAccountWebApp
    {
        Task<UserResponseDto> ForgoutPasswordAsync(ForgoutPasswordDto forgoutPasswordDto);
        Task<UserResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<UserResponseDto> LoginUser(LoginDto loginDto);
        Task<string> ConfirmAccountByEmailAsync(string token, string userId);
        Task<UserResponseDto> ResendActivationEmailAsync(ResendActivationEmailDto resendActivationEmailDto);
    }
}
