using System.Collections.Generic;
using RealEstateApp.Core.Application.Contracts.Admin;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services.Admin
{
    public sealed class AdministratorValidationService : IAdministratorValidationService
    {
        public ValidationResult ValidateSelfEdit(EditInternalUserDto dto, string currentAdminId)
        {
            var errors = new List<Error>();
            
            if (dto.Id == currentAdminId)
            {
                errors.Add(ErrorAdministrator.SelfEdit);
            }
            
            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public ValidationResult ValidateSelfInactivation(AlterStateUserDto dto, string currentAdminId)
        {
            var errors = new List<Error>();
            
            if (dto.Id == currentAdminId && !dto.State)
            {
                errors.Add(ErrorAdministrator.SelfInactivation);
            }
            
            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
