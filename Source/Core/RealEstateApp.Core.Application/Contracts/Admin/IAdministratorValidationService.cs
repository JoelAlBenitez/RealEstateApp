using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.DTOs.Users.Operational;

namespace RealEstateApp.Core.Application.Contracts.Admin
{
    public interface IAdministratorValidationService
    {
        ValidationResult ValidateSelfInactivation(AlterStateUserDto dto, string currentAdminId);
    }
}
