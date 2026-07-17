using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.Contracts.Admin;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.Contracts.Users.Base;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services.Admin
{
    public sealed class AdministratorValidationService : IAdministratorValidationService
    {
        private readonly IOperationalAccountWebApi _internalAccountApi;

        public AdministratorValidationService(IOperationalAccountWebApi internalAccountApi)
        {
            _internalAccountApi = internalAccountApi;
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

        public async Task<ValidationResult> ValidateMinimumActiveAdmin(AlterStateUserDto dto)
        {
            var errors = new List<Error>();
            // Solo aplica cuando se está INACTIVANDO (dto.State == false)
            if (!dto.State)
            {
                var admins = await _internalAccountApi.GetAllInternalUsersByRol(Roles.Administrador);
                var activeAdminsCount = admins.Count(a => a.State);
                var isTargetCurrentlyActive = admins.Any(a => a.Id == dto.Id && a.State);
                if (activeAdminsCount <= 1 && isTargetCurrentlyActive)
                {
                    errors.Add(ErrorAdministrator.LastAdminRequired);
                }
            }
            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
