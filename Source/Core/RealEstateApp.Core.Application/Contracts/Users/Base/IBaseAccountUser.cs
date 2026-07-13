using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Core.Application.Contracts.Users.Base
{
    public interface IBaseAccountUser
    {
        Task<UserResponseDto> ChangeStateAsync(AlterStateUserDto alterStateUserDto);
        Task<UserResponseDto> DeleteAsync(string IdUser);
        Task<BaseGetUserDto> GetUserBaseById(string IdUser);
        Task SignOutAsync();

    }
}
