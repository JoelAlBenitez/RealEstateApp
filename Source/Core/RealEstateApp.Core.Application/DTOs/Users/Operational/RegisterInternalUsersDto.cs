using RealEstateApp.Core.Application.DTOs.Users.Operational.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.Operational
{
    public sealed record RegisterInternalUsersDto : RegisterUserDto
    {
        public required string IDCard { get; set; }

    }
}
