using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.SaleType;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.DTOs.SaleType;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.SaleType
{
    public sealed class SaleTypeService : GenericServices<SaveSaleTypeDto, RealEstateApp.Core.Domain.Entities.SaleType, int>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly ISaleTypeValidationService _saleTypeValidationService;
        private readonly IPropertyService _propertyService;

        public SaleTypeService(
            ISaleTypeRepository saleTypeRepository, 
            IMapper mapper, 
            ISaleTypeValidationService saleTypeValidationService,
            IPropertyService propertyService)
            : base(saleTypeRepository, mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _saleTypeValidationService = saleTypeValidationService;
            _propertyService = propertyService;
        }

        public async Task<ValidationResult<IReadOnlyCollection<SaleTypeDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _saleTypeRepository.GetAllAsync();
                var dtos = base._mapper.Map<List<SaleTypeDto>>(entities);

                // Nota: se usa un conteo individual por elemento ejecutado en paralelo con Task.WhenAll
                // en vez de una consulta agrupada. Confirmado con el líder técnico (Joel) que esto es
                // aceptable para catálogos maestros con pocos registros (no es un problema de rendimiento en este contexto).
                var countTasks = dtos.Select(async dto =>
                {
                    var countResult = await _propertyService.CountBySaleTypeAsync(dto.Id);
                    dto.PropertyCount = countResult.IsValid ? countResult.Value : 0;
                }).ToList();

                await Task.WhenAll(countTasks);
                
                return ValidationResult<IReadOnlyCollection<SaleTypeDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<SaleTypeDto>>.Failure(
                    new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<TypeSale>>> GetAllForSelectAsync()
        {
            try
            {
                var entities = await _saleTypeRepository.GetAllAsync();
                var result = entities.Select(e => new TypeSale { Id = e.Id, Name = e.Name }).ToList();
                return ValidationResult<IReadOnlyCollection<TypeSale>>.Success(result);
            }
            catch (Exception ex)
            {
                return ValidationResult<IReadOnlyCollection<TypeSale>>.Failure(
                    new Error("SaleTypeService.GetAllForSelectAsync", ex.Message));
            }
        }

        public override async Task<ValidationResult> AddAsync(SaveSaleTypeDto dto)
        {
            var validationResult = await _saleTypeValidationService.ValidateForCreateAsync(dto);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            dto = dto with { 
                Name = dto.Name.Trim(), 
                Description = dto.Description.Trim() 
            };
            
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult?> UpdateAsync(SaveSaleTypeDto dto)
        {
            var validationResult = await _saleTypeValidationService.ValidateForUpdateAsync(dto);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            dto = dto with { 
                Name = dto.Name.Trim(), 
                Description = dto.Description.Trim() 
            };
            
            return await base.UpdateAsync(dto);
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            var cascadeResult = await _propertyService.DeletePropertiesBySaleTypeAsync(id);
            if (!cascadeResult.IsValid)
            {
                return cascadeResult;
            }
            return await base.RemoveAsync(id);
        }
    }
}
