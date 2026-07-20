using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.DTOs.PropertyType;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.PropertyType
{
    public sealed class PropertyTypeValidationService : IPropertyTypeValidationService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IUserSession _userSession;

        public PropertyTypeValidationService(IPropertyTypeRepository propertyTypeRepository, IUserSession userSession)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _userSession = userSession;
        }

        private void ValidateAdminRole(List<Error> errors)
        {
            var roles = _userSession.GetRolesCurrentUser();
            if (roles == null || !roles.Contains(Roles.Administrador.ToString()))
            {
                errors.Add(ErrorPropertyType.Forbidden);
            }
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SavePropertyTypeDto dto)
        {
            var errors = new List<Error>();
            
            ValidateAdminRole(errors);

            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                errors.Add(ErrorPropertyType.RequiredFields);
            }
            else
            {
                bool nameExists = await _propertyTypeRepository.ExistNameAsync(trimmedName, 0);
                
                if (nameExists)
                {
                    errors.Add(ErrorPropertyType.NameDuplicate);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(SavePropertyTypeDto dto)
        {
            var errors = new List<Error>();
            
            ValidateAdminRole(errors);

            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                errors.Add(ErrorPropertyType.RequiredFields);
            }
            else
            {
                bool nameExists = await _propertyTypeRepository.ExistNameAsync(trimmedName, dto.Id ?? 0);
                
                if (nameExists)
                {
                    errors.Add(ErrorPropertyType.NameDuplicateEdit);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
