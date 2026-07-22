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
        private readonly IPropertyCascadeService _propertyCascadeService;

        public SaleTypeService(
            ISaleTypeRepository saleTypeRepository, 
            IMapper mapper, 
            ISaleTypeValidationService saleTypeValidationService,
            IPropertyCascadeService propertyCascadeService)
            : base(saleTypeRepository, mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _saleTypeValidationService = saleTypeValidationService;
            _propertyCascadeService = propertyCascadeService;
        }

        public async Task<ValidationResult<IReadOnlyCollection<SaleTypeDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _saleTypeRepository.GetAllAsync();
                var dtos = base._mapper.Map<List<SaleTypeDto>>(entities);

               
                var countsResult = await _propertyCascadeService.GetPropertyCountsBySaleTypeAsync();
                var counts = countsResult.IsValid && countsResult.Value != null
                    ? countsResult.Value
                    : new Dictionary<int, int>();

                foreach (var dto in dtos)
                {
                    dto.PropertyCount = counts.TryGetValue(dto.Id, out var count) ? count : 0;
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

            var entity = new RealEstateApp.Core.Domain.Entities.SaleType
            {
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim(),
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow
            };

            await _saleTypeRepository.AddAsync(entity);
            var result = await _saleTypeRepository.SaveAsync();
            return result > 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(new Error("Oops", "Ocurrió un error al procesar la solicitud. Intente nuevamente más tarde."));
        }

        public override async Task<ValidationResult?> UpdateAsync(SaveSaleTypeDto dto)
        {
            var validationResult = await _saleTypeValidationService.ValidateForUpdateAsync(dto);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var entity = await _saleTypeRepository.GetByIdAsync(dto.Id!.Value);
            if (entity == null)
            {
                return ValidationResult.Failure(new Error("SaleType.NotFound", "El tipo de venta a actualizar no existe."));
            }

            // Se conserva CreateAt original; solo se actualiza UpdateAt.
            entity.Name = dto.Name.Trim();
            entity.Description = dto.Description.Trim();
            entity.UpdateAt = DateTimeOffset.UtcNow;

            var result = await _saleTypeRepository.SaveAsync();
            return result > 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(new Error("Oops", "Ocurrió un error al actualizar el elemento. Intente nuevamente más tarde."));
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            var cascadeResult = await _propertyCascadeService.DeletePropertiesBySaleTypeAsync(id);
            if (!cascadeResult.IsValid)
            {
                return cascadeResult;
            }
            return await base.RemoveAsync(id);
        }
    }
}
