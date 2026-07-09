using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Core.Application.Contracts.Users.Base
{
    public interface IBaseAccountUser
    {
        Task<UserResponseDto> CreateInternalAsync(RegisterInternalUsersDto registerUserDto, bool isApi);
        Task<UserResponseDto> CreateExternalAsync(RegisterExternalUsers registerUserDto);
        Task<EditResponseDto> UpdateAgentAsync(EditAgentUserDto editAgent);
        Task<EditResponseDto> UpdateInternalAsync(EditInernalUserDto edit);
        Task<UserResponseDto> ChangeStateAsync(AlterStateUserDto alterStateUserDto, string IdUserCurrent);
        Task<UserResponseDto> DeleteAsync(string IdUser);
        Task<BaseGetUserDto> GetById(string IdUser);
        Task SignOutAsync();

    }
}
