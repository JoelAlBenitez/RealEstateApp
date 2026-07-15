using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.Contracts.Improvement;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.Improvement
{
    public sealed class ImprovementValidationService : IImprovementValidationService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IUserSession _userSession;

        public ImprovementValidationService(IImprovementRepository improvementRepository, IUserSession userSession)
        {
            _improvementRepository = improvementRepository;
            _userSession = userSession;
        }

        private void ValidateAdminRole(List<Error> errors)
        {
            var roles = _userSession.GetRolesCurrentUser();
            if (roles == null || !roles.Contains(Roles.Administrador.ToString()))
            {
                errors.Add(ErrorImprovement.Forbidden);
            }
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveImprovementDto dto)
        {
            var errors = new List<Error>();
            
            ValidateAdminRole(errors);

            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                errors.Add(ErrorImprovement.RequiredFields);
            }
            else
            {
                var existing = await _improvementRepository.GetAllAsync();
                bool nameExists = existing.Any(e => e.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
                
                if (nameExists)
                {
                    errors.Add(ErrorImprovement.NameDuplicate);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(SaveImprovementDto dto)
        {
            var errors = new List<Error>();
            
            ValidateAdminRole(errors);

            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                errors.Add(ErrorImprovement.RequiredFields);
            }
            else
            {
                var existing = await _improvementRepository.GetAllAsync();
                bool nameExists = existing.Any(e =>
                    e.Id != dto.Id &&
                    e.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
                
                if (nameExists)
                {
                    errors.Add(ErrorImprovement.NameDuplicateEdit);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
