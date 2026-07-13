using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.DTOs.PropertyType;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.PropertyType
{
    public sealed class PropertyTypeService : GenericServices<SavePropertyTypeDto, RealEstateApp.Core.Domain.Entities.PropertyType, int>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
            : base(propertyTypeRepository, mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyTypeDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _propertyTypeRepository.GetAllAsync();
                var dtos = new List<PropertyTypeDto>();
                foreach (var entity in entities)
                {
                    var count = await _propertyTypeRepository.CountByPropertyTypeAsync(entity.Id);
                    dtos.Add(new PropertyTypeDto
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        PropertyCount = count
                    });
                }
                return ValidationResult<IReadOnlyCollection<PropertyTypeDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<PropertyTypeDto>>.Failure(
                    new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public override async Task<ValidationResult> AddAsync(SavePropertyTypeDto dto)
        {
            // Validación de negocio del documento funcional: nombre requerido, sin
            // espacios en blanco, y único (no registrado previamente)
            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                return ValidationResult.Failure(ErrorPropertyType.RequiredFields);
            }
            
            var existing = await _propertyTypeRepository.GetAllAsync();
            bool nameExists = existing.Any(e => e.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
            
            if (nameExists)
            {
                return ValidationResult.Failure(ErrorPropertyType.NameDuplicate);
            }
            
            dto = dto with { Name = trimmedName, Description = dto.Description?.Trim() ?? string.Empty };
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult?> UpdateAsync(SavePropertyTypeDto dto)
        {
            var trimmedName = dto.Name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmedName))
            {
                return ValidationResult.Failure(ErrorPropertyType.RequiredFields);
            }
            
            var existing = await _propertyTypeRepository.GetAllAsync();
            bool nameExists = existing.Any(e =>
                e.Id != dto.Id &&
                e.Name.Trim().Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
            
            if (nameExists)
            {
                return ValidationResult.Failure(ErrorPropertyType.NameDuplicateEdit);
            }
            
            dto = dto with { Name = trimmedName, Description = dto.Description?.Trim() ?? string.Empty };
            return await base.UpdateAsync(dto);
        }

        // PENDIENTE DE CONFIRMAR CON SEBASTIÁN: la eliminación en cascada de propiedades
        // al borrar un tipo de propiedad depende de que Sebastián configure la FK
        // Property.PropertyTypeId con OnDelete(DeleteBehavior.Cascade) en Fluent API
        // (según acuerdo del equipo en distribucion-equipo.html línea 414). Sebastián
        // aún no ha llegado a esa parte de su implementación. Mientras tanto, RemoveAsync
        // hereda el comportamiento genérico de GenericServices (solo borra el registro
        // de PropertyType) — NO purga las propiedades asociadas todavía. Si se elimina
        // un tipo de propiedad con propiedades activas antes de que Sebastián configure
        // la cascada, quedarán registros huérfanos con una FK inválida.
    }
}
