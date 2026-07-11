using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;

namespace RealEstateApp.Core.Application.Contracts.Users.Validation
{
    public interface IServicesValidateUsers
    {
        Task<UserResponseDto> CreateInternalValidateUserAsync(
                RegisterInternalUsersDto internalUsersDto,
            UserResponseDto response
            );
       Task<UserResponseDto> CreateExternalValidateUserAsync(
            RegisterExternalUsersDto externalUsersDto,
            UserResponseDto  response
            );
        Task<EditResponseDto> UpdateInternalValidateUserAsync (
            EditInternalUserDto editInternalUserDto,
            UserResponseDto response
            );
        Task<EditResponseDto> UpdateExternalValidateUserAsync(
            EditAgentUserDto editAgentUserDto,
            EditResponseDto response
            );
        
    }
}
