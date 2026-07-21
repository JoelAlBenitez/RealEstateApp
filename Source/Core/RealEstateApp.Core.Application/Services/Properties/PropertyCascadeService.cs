using Microsoft.Extensions.Logging;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyCascadeService : IPropertyCascadeService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFileManager _fileManager;
        private readonly ILogger<PropertyCascadeService> _logger;

        public PropertyCascadeService(
            IPropertyRepository propertyRepository,
            IFileManager fileManager,
            ILogger<PropertyCascadeService> logger)
        {
            _propertyRepository = propertyRepository;
            _fileManager = fileManager;
            _logger = logger;
        }

        public async Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync()
        {
            try
            {
                var availableCount = await _propertyRepository.GetPropertiesCountByStatusGlobalAsync(PropertyState.Available);
                var soldCount = await _propertyRepository.GetPropertiesCountByStatusGlobalAsync(PropertyState.Sold);

                var totals = new PropertyTotalsDto
                {
                    AvailableProperties = availableCount,
                    SoldProperties = soldCount
                };
                return ValidationResult<PropertyTotalsDto>.Success(totals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult<PropertyTotalsDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId)
        {
            try
            {
                var properties = await _propertyRepository.GetPropertiesByAgentWithImagesAsync(agentId);
                foreach (var property in properties)
                {
                    if (property.Images != null && property.Images.Any())
                    {
                        var ids = property.Images
                            .Select(img => System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(img.ImageUrl)))
                            .Where(name => !string.IsNullOrEmpty(name))
                            .ToList();
                        await _fileManager.DeleteManyAsync(ids!, "Properties");
                    }
                    await _propertyRepository.DeleteAsync(property);
                }
                return ValidationResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult<int>> CountByPropertyTypeAsync(int propertyTypeId)
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var count = properties.Count(p => p.PropertyTypeId == propertyTypeId);
                return ValidationResult<int>.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
            }
        }

        public async Task<ValidationResult<int>> CountBySaleTypeAsync(int saleTypeId)
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var count = properties.Count(p => p.SaleTypeId == saleTypeId);
                return ValidationResult<int>.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
            }
        }

        public async Task<ValidationResult<int>> CountByImprovementAsync(int improvementId)
        {
            try
            {
                return await Task.FromResult(ValidationResult<int>.Success(0));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<int>>> GetPropertyIdsByTypeAsync(int propertyTypeId)
        {
            try
            {
                var ids = await _propertyRepository.GetPropertyIdsByTypeAsync(propertyTypeId);
                return ValidationResult<IReadOnlyCollection<int>>.Success(ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult<IReadOnlyCollection<int>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<int>>> GetPropertyIdsBySaleTypeAsync(int saleTypeId)
        {
            try
            {
                var ids = await _propertyRepository.GetPropertyIdsBySaleTypeAsync(saleTypeId);
                return ValidationResult<IReadOnlyCollection<int>>.Success(ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult<IReadOnlyCollection<int>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
            }
        }

        public async Task<ValidationResult> DeletePropertiesByTypeAsync(int propertyTypeId)
        {
            try
            {
                var properties = await _propertyRepository.GetPropertiesByPropertyTypeWithImagesAsync(propertyTypeId);
                foreach (var property in properties)
                {
                    if (property.Images != null && property.Images.Any())
                    {
                        var ids = property.Images
                            .Select(img => System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(img.ImageUrl)))
                            .Where(name => !string.IsNullOrEmpty(name))
                            .ToList();
                        await _fileManager.DeleteManyAsync(ids!, "Properties");
                    }
                    await _propertyRepository.DeleteAsync(property);
                }
                return ValidationResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento."));
            }
        }

        public async Task<ValidationResult> DeletePropertiesBySaleTypeAsync(int saleTypeId)
        {
            try
            {
                var properties = await _propertyRepository.GetPropertiesBySaleTypeWithImagesAsync(saleTypeId);
                foreach (var property in properties)
                {
                    if (property.Images != null && property.Images.Any())
                    {
                        var ids = property.Images
                            .Select(img => System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(img.ImageUrl)))
                            .Where(name => !string.IsNullOrEmpty(name))
                            .ToList();
                        await _fileManager.DeleteManyAsync(ids!, "Properties");
                    }
                    await _propertyRepository.DeleteAsync(property);
                }
                return ValidationResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyCascadeService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento."));
            }
        }
    }
}
