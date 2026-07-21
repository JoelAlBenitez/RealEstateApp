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

                // Una sola consulta de asociaciones y conteo agrupado en memoria.
                // Evita el N+1 (una consulta por mejora) y, sobre todo, evita paralelizar
                // consultas sobre el mismo DbContext (Scoped), que lanza
                // "A second operation was started on this context instance...".
                var allAssociations = await _propertyImprovementRepository.GetAllAsync();
                var counts = allAssociations
                    .GroupBy(pi => pi.ImprovementId)
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (var dto in dtos)
                {
                    dto.PropertyCount = counts.TryGetValue(dto.Id, out var count) ? count : 0;
                }

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

            var entity = new RealEstateApp.Core.Domain.Entities.Improvement
            {
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim(),
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow
            };

            await _improvementRepository.AddAsync(entity);
            var result = await _improvementRepository.SaveAsync();
            return result > 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(new Error("Oops", "Ocurrió un error al procesar la solicitud. Intente nuevamente más tarde."));
        }

        public override async Task<ValidationResult?> UpdateAsync(SaveImprovementDto dto)
        {
            var validationResult = await _improvementValidationService.ValidateForUpdateAsync(dto);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var entity = await _improvementRepository.GetByIdAsync(dto.Id!.Value);
            if (entity == null)
            {
                return ValidationResult.Failure(new Error("Improvement.NotFound", "La mejora a actualizar no existe."));
            }

            // Se conserva CreateAt original; solo se actualiza UpdateAt.
            entity.Name = dto.Name.Trim();
            entity.Description = dto.Description.Trim();
            entity.UpdateAt = DateTimeOffset.UtcNow;

            var result = await _improvementRepository.SaveAsync();
            return result > 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(new Error("Oops", "Ocurrió un error al actualizar el elemento. Intente nuevamente más tarde."));
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
