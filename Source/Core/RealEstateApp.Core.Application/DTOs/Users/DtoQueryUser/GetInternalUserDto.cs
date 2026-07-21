using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser
{
    public sealed class GetInternalUserDto : BaseGetUserDto
    {
        public required string IDCard { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required bool State {  get; set; }
        
    }
}
