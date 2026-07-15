using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Improvement;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.Improvement
{
    public sealed class ImprovementService : GenericServices<SaveImprovementDto, RealEstateApp.Core.Domain.Entities.Improvement, int>, IImprovementService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IImprovementValidationService _improvementValidationService;

        public ImprovementService(
            IImprovementRepository improvementRepository, 
            IMapper mapper, 
            IImprovementValidationService improvementValidationService)
            : base(improvementRepository, mapper)
        {
            _improvementRepository = improvementRepository;
            _improvementValidationService = improvementValidationService;
        }

        public async Task<ValidationResult<IReadOnlyCollection<ImprovementDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _improvementRepository.GetAllAsync();
                
                // TODO: Falta un método nuevo en IImprovementRepository que traiga la entidad 
                // + el conteo en una sola consulta (GroupBy/Join) en vez del foreach actual con N+1 consultas.
                // Mientras tanto se usa AutoMapper para el mapeo simple de Id/Name/Description,
                // dejando PropertyCount en 0 (valor por defecto) con este TODO explicado.
                var dtos = base._mapper.Map<List<ImprovementDto>>(entities);
                
                return ValidationResult<IReadOnlyCollection<ImprovementDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<ImprovementDto>>.Failure(
                    new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public override async Task<ValidationResult> AddAsync(SaveImprovementDto dto)
        {
            var validationResult = await _improvementValidationService.ValidateForCreateAsync(dto);
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

        public override async Task<ValidationResult?> UpdateAsync(SaveImprovementDto dto)
        {
            var validationResult = await _improvementValidationService.ValidateForUpdateAsync(dto);
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
    }
}
