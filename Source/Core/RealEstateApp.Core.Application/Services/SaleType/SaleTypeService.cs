using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.SaleType;
using RealEstateApp.Core.Application.DTOs.SaleType;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Core.Application.Services.SaleType
{
    public sealed class SaleTypeService : GenericServices<SaveSaleTypeDto, RealEstateApp.Core.Domain.Entities.SaleType, int>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IUserSession _userSession;

        public SaleTypeService(ISaleTypeRepository saleTypeRepository, IMapper mapper, IUserSession userSession)
            : base(saleTypeRepository, mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _userSession = userSession;
        }

        private ValidationResult? ValidateAdminRole()
        {
            var roles = _userSession.GetRolesCurrentUser();
            if (roles == null || !roles.Contains(Roles.Administrador.ToString()))
            {
                return ValidationResult.Failure(new Error("Forbidden", "No tiene permisos para realizar esta acción."));
            }
            return null;
        }

        public async Task<ValidationResult<IReadOnlyCollection<SaleTypeDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _saleTypeRepository.GetAllAsync();
                var dtos = new List<SaleTypeDto>();
                foreach (var entity in entities)
                {
                    var count = await _saleTypeRepository.CountBySaleTypeAsync(entity.Id);
                    dtos.Add(new SaleTypeDto
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        PropertyCount = count
                    });
                }
                return ValidationResult<IReadOnlyCollection<SaleTypeDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<SaleTypeDto>>.Failure(
                    new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public override async Task<ValidationResult> AddAsync(SaveSaleTypeDto dto)
        {
            var roleCheck = ValidateAdminRole();
            if (roleCheck != null) return roleCheck;

            // Validación de negocio del documento funcional: nombre requerido, sin
            // espacios en blanco, y único (no registrado previamente)
            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                return ValidationResult.Failure(ErrorSaleType.RequiredFields);
            }
            
            var existing = await _saleTypeRepository.GetAllAsync();
            bool nameExists = existing.Any(e => e.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
            
            if (nameExists)
            {
                return ValidationResult.Failure(ErrorSaleType.NameDuplicate);
            }
            
            dto = dto with { Name = trimmedName, Description = dto.Description?.Trim() ?? string.Empty };
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult?> UpdateAsync(SaveSaleTypeDto dto)
        {
            var roleCheck = ValidateAdminRole();
            if (roleCheck != null) return roleCheck;

            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                return ValidationResult.Failure(ErrorSaleType.RequiredFields);
            }
            
            var existing = await _saleTypeRepository.GetAllAsync();
            bool nameExists = existing.Any(e =>
                e.Id != dto.Id &&
                e.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
            
            if (nameExists)
            {
                return ValidationResult.Failure(ErrorSaleType.NameDuplicateEdit);
            }
            
            dto = dto with { Name = trimmedName, Description = dto.Description?.Trim() ?? string.Empty };
            return await base.UpdateAsync(dto);
        }

        // PENDIENTE DE CONFIRMAR CON SEBASTIÁN: la eliminación en cascada de propiedades
        // al borrar un tipo de venta depende de que Sebastián configure la FK
        // Property.SaleTypeId con OnDelete(DeleteBehavior.Cascade) en Fluent API
        // (según acuerdo del equipo en distribucion-equipo.html línea 414). Sebastián
        // aún no ha llegado a esa parte de su implementación. Mientras tanto, RemoveAsync
        // hereda el comportamiento genérico de GenericServices (solo borra el registro
        // de SaleType) — NO purga las propiedades asociadas todavía. Si se elimina
        // un tipo de venta con propiedades activas antes de que Sebastián configure
        // la cascada, quedarán registros huérfanos con una FK inválida.
    }
}
