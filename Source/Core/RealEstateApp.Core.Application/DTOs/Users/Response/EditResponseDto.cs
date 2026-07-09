using RealEstateApp.Core.Application.DTOs.Users.BaseError;

namespace RealEstateApp.Core.Application.DTOs.Users.Response
{
    public sealed record EditResponseDto : BaseErrors
    {
        public required bool IsChangePassword { get; set; }
    }
}
