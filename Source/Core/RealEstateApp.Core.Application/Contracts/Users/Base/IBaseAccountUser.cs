using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Core.Application.Contracts.Users.Base
{
    public interface IBaseAccountUser
    {
        Task<UserResponseDto> CreateAsync(RegisterUserDto registerUserDto, bool isApi);
        Task<EditResponseDto> UpdateAsync(EditUserDto editUserDto);
        Task<UserResponseDto> ChangeStateAsync(AlterStateUserDto alterStateUserDto, string IdUserCurrent);
        Task<UserResponseDto> DeleteAsync(string IdUser);
        Task<BaseGetUserDto> GetById(string IdUser);
    }
}
