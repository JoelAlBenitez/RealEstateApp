using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.SaleType;
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

        public SaleTypeService(
            ISaleTypeRepository saleTypeRepository, 
            IMapper mapper, 
            ISaleTypeValidationService saleTypeValidationService)
            : base(saleTypeRepository, mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _saleTypeValidationService = saleTypeValidationService;
        }

        public async Task<ValidationResult<IReadOnlyCollection<SaleTypeDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _saleTypeRepository.GetAllAsync();
                
                // TODO: Falta un método nuevo en ISaleTypeRepository que traiga la entidad 
                // + el conteo en una sola consulta (GroupBy/Join) en vez del foreach actual con N+1 consultas.
                // Mientras tanto se usa AutoMapper para el mapeo simple de Id/Name/Description,
                // dejando PropertyCount en 0 (valor por defecto) con este TODO explicado.
                var dtos = base._mapper.Map<List<SaleTypeDto>>(entities);
                
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
                Name = dto.Name?.Trim() ?? string.Empty, 
                Description = dto.Description?.Trim() ?? string.Empty 
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
                Name = dto.Name?.Trim() ?? string.Empty, 
                Description = dto.Description?.Trim() ?? string.Empty 
            };
            
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
