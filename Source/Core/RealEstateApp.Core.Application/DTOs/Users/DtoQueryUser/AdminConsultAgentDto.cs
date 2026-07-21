using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;

namespace RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser
{
   public sealed class AdminConsultAgentDto : BaseGetUserDto
    {
        public required string Email { get; set; }
        public required int  Properties { get; set; }
        public required bool State {  get; set; }
    }
}
