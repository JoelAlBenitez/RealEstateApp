using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Core.Application.Contracts.Users.InternalUsers
{
    public interface IAutheAccountWebApi
    {
        Task<UserResponseDto> LoginAsync(LoginDto loginDto);
    }
}
