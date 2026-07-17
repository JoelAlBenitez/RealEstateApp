using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.DTOs.PropertyType;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.PropertyType
{
    public sealed class PropertyTypeService : GenericServices<SavePropertyTypeDto, RealEstateApp.Core.Domain.Entities.PropertyType, int>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IPropertyTypeValidationService _propertyTypeValidationService;

        public PropertyTypeService(
            IPropertyTypeRepository propertyTypeRepository, 
            IMapper mapper, 
            IPropertyTypeValidationService propertyTypeValidationService)
            : base(propertyTypeRepository, mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _propertyTypeValidationService = propertyTypeValidationService;
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyTypeDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _propertyTypeRepository.GetAllAsync();
                
                // TODO: Falta un método nuevo en IPropertyTypeRepository que traiga la entidad 
                // + el conteo en una sola consulta (GroupBy/Join) en vez del foreach actual con N+1 consultas.
                // Mientras tanto se usa AutoMapper para el mapeo simple de Id/Name/Description,
                // dejando PropertyCount en 0 (valor por defecto) con este TODO explicado.
                var dtos = _mapper.Map<List<PropertyTypeDto>>(entities);
                
                return ValidationResult<IReadOnlyCollection<PropertyTypeDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<PropertyTypeDto>>.Failure(
                    new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<TypeProperty>>> GetAllForSelectAsync()
        {
            var entities = await _propertyTypeRepository.GetAllAsync();
            var result = entities.Select(e => new TypeProperty { Id = e.Id, Name = e.Name }).ToList();
            return ValidationResult<IReadOnlyCollection<TypeProperty>>.Success(result);
        }

        public override async Task<ValidationResult> AddAsync(SavePropertyTypeDto dto)
        {
            var validationResult = await _propertyTypeValidationService.ValidateForCreateAsync(dto);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            dto = dto with { 
                Name = dto.Name?.Trim() ?? string.Empty, 
                Description = dto.Description?.Trim() ?? string.Empty 
            };
            
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult?> UpdateAsync(SavePropertyTypeDto dto)
        {
            var validationResult = await _propertyTypeValidationService.ValidateForUpdateAsync(dto);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            dto = dto with { 
                Name = dto.Name?.Trim() ?? string.Empty, 
                Description = dto.Description?.Trim() ?? string.Empty 
            };
            
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
