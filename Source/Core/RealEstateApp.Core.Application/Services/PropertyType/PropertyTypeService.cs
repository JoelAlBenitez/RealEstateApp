
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.Contracts.Properties;
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
        private readonly IPropertyService _propertyService;

        public PropertyTypeService(
            IPropertyTypeRepository propertyTypeRepository, 
            IMapper mapper, 
            IPropertyTypeValidationService propertyTypeValidationService,
            IPropertyService propertyService)
            : base(propertyTypeRepository, mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _propertyTypeValidationService = propertyTypeValidationService;
            _propertyService = propertyService;
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyTypeDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _propertyTypeRepository.GetAllAsync();
                var dtos = _mapper.Map<List<PropertyTypeDto>>(entities);
                
                foreach (var dto in dtos)
                {
                    var countResult = await _propertyService.CountByPropertyTypeAsync(dto.Id);
                    dto.PropertyCount = countResult.IsValid ? countResult.Value : 0;
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

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            var cascadeResult = await _propertyService.DeletePropertiesByTypeAsync(id);
            if (!cascadeResult.IsValid)
            {
                return cascadeResult;
            }
            return await base.RemoveAsync(id);
        }
    }
}
