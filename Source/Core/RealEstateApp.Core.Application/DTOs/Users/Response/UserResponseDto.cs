using RealEstateApp.Core.Application.DTOs.Users.BaseError;

namespace RealEstateApp.Core.Application.DTOs.Users.Response
{
    public sealed class UserResponseDto : BaseErrors { 
        public required List<string> Roles { get; set; }
      
    }
    
}
