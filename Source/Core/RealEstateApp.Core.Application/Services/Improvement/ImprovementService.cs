using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.DTOs.Property;
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
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;

        public ImprovementService(
            IImprovementRepository improvementRepository, 
            IMapper mapper, 
            IImprovementValidationService improvementValidationService,
            IPropertyImprovementRepository propertyImprovementRepository)
            : base(improvementRepository, mapper)
        {
            _improvementRepository = improvementRepository;
            _improvementValidationService = improvementValidationService;
            _propertyImprovementRepository = propertyImprovementRepository;
        }

        public async Task<ValidationResult<IReadOnlyCollection<ImprovementDto>>> GetAllWithCountAsync()
        {
            try
            {
                var entities = await _improvementRepository.GetAllAsync();
                var dtos = _mapper.Map<List<ImprovementDto>>(entities);
                
                // Nota: se usa un conteo individual por elemento ejecutado en paralelo con Task.WhenAll
                // en vez de una consulta agrupada. Confirmado con el líder técnico (Joel) que esto es
                // aceptable para catálogos maestros con pocos registros (no es un problema de rendimiento en este contexto).
                var countTasks = dtos.Select(async dto =>
                {
                    dto.PropertyCount = await _improvementRepository.CountByImprovementAsync(dto.Id);
                }).ToList();
                
                await Task.WhenAll(countTasks);
                
                return ValidationResult<IReadOnlyCollection<ImprovementDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<ImprovementDto>>.Failure(
                    new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente de nuevo más tarde.")
                );
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<TypeImprovement>>> GetAllForSelectAsync()
        {
            try
            {
                var entities = await _improvementRepository.GetAllAsync();
                var result = entities.Select(e => new TypeImprovement { Id = e.Id, Name = e.Name }).ToList();
                return ValidationResult<IReadOnlyCollection<TypeImprovement>>.Success(result);
            }
            catch (Exception ex)
            {
                return ValidationResult<IReadOnlyCollection<TypeImprovement>>.Failure(
                    new Error("ImprovementService.GetAllForSelectAsync", ex.Message));
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
                Name = dto.Name.Trim(), 
                Description = dto.Description.Trim() 
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
                Name = dto.Name.Trim(), 
                Description = dto.Description.Trim() 
            };
            
            return await base.UpdateAsync(dto);
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            var allAssociations = await _propertyImprovementRepository.GetAllAsync();
            var associationsToRemove = allAssociations.Where(pi => pi.ImprovementId == id).ToList();
            foreach (var association in associationsToRemove)
            {
                await _propertyImprovementRepository.DeleteAsync(association);
            }
            return await base.RemoveAsync(id);
        }
    }
}
