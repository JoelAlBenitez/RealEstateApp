using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.Contracts.SaleType;
using RealEstateApp.Core.Application.DTOs.SaleType;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.SaleType
{
    public sealed class SaleTypeValidationService : ISaleTypeValidationService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IUserSession _userSession;

        public SaleTypeValidationService(ISaleTypeRepository saleTypeRepository, IUserSession userSession)
        {
            _saleTypeRepository = saleTypeRepository;
            _userSession = userSession;
        }

        private void ValidateAdminRole(List<Error> errors)
        {
            var roles = _userSession.GetRolesCurrentUser();
            if (roles == null || !roles.Contains(Roles.Administrador.ToString()))
            {
                errors.Add(ErrorSaleType.Forbidden);
            }
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveSaleTypeDto dto)
        {
            var errors = new List<Error>();
            
            ValidateAdminRole(errors);

            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                errors.Add(ErrorSaleType.RequiredFields);
            }
            else
            {
                bool nameExists = await _saleTypeRepository.ExistNameAsync(trimmedName, 0);
                
                if (nameExists)
                {
                    errors.Add(ErrorSaleType.NameDuplicate);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(SaveSaleTypeDto dto)
        {
            var errors = new List<Error>();
            
            ValidateAdminRole(errors);

            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                errors.Add(ErrorSaleType.RequiredFields);
            }
            else
            {
                bool nameExists = await _saleTypeRepository.ExistNameAsync(trimmedName, dto.Id ?? 0);
                
                if (nameExists)
                {
                    errors.Add(ErrorSaleType.NameDuplicateEdit);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
