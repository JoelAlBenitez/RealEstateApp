using RealEstateApp.Core.Application.Contracts.Users.Base;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Infraestructure.Identity.Services.Base
{
    public sealed class BaseAccountServices : IBaseAccountUser
    {
        
        public Task<UserResponseDto> ChangeStateAsync(AlterStateUserDto alterStateUserDto, string IdUserCurrent)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDto> CreateAsync(RegisterUserDto registerUserDto, bool isApi)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponseDto> DeleteAsync(string IdUser)
        {
            throw new NotImplementedException();
        }

        public Task<BaseGetUserDto> GetById(string IdUser)
        {
            throw new NotImplementedException();
        }

        public Task<EditResponseDto> UpdateAsync(EditUserDto editUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
