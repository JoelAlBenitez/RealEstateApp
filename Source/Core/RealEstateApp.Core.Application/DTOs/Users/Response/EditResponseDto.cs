using RealEstateApp.Core.Application.DTOs.Users.BaseError;

namespace RealEstateApp.Core.Application.DTOs.Users.Response
{
    public sealed class EditResponseDto : BaseErrors
    {
        public required bool IsChangePassword { get; set; }
    }
}
