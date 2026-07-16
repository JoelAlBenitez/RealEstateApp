using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Application.Contracts.FileManager;
using Microsoft.Extensions.Logging;

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyService : GenericServices<SavePropertyDto, Property, int>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyValidationService _validationService;
        private readonly IFileManager _fileManager;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly ILogger<PropertyService> _logger;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IPropertyValidationService validationService,
            IMapper mapper,
            IFileManager fileManager,
            IPropertyImprovementRepository propertyImprovementRepository,
            ILogger<PropertyService> _logger)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _validationService = validationService;
            _fileManager = fileManager;
            _propertyImprovementRepository = propertyImprovementRepository;
            this._logger = _logger;
        }

        public override async Task<ValidationResult> AddAsync(SavePropertyDto dto)
        {
            try
            {
                var validation = await _validationService.ValidateForCreateAsync(dto);
                if (!validation.IsValid)
                {
                    return validation;
                }

                string code;
                bool codeExists;
                var random = new Random();
                do
                {
                    code = random.Next(100000, 999999).ToString();
                    codeExists = await _propertyRepository.ExistsCodeAsync(code);
                } while (codeExists);
                dto.Code = code;

                var property = _mapper.Map<Property>(dto);
                property.CreateAt = DateTimeOffset.UtcNow;
                property.UpdateAt = DateTimeOffset.UtcNow;

                var improvements = new List<PropertyImprovement>();
                if (dto.ImprovementIds != null && dto.ImprovementIds.Any())
                {
                    foreach (var improvementId in dto.ImprovementIds)
                    {
                        improvements.Add(new PropertyImprovement
                        {
                            PropertyId = property.Id,
                            ImprovementId = improvementId
                        });
                    }
                }
                property.PropertyImprovements = improvements;

                var images = new List<PropertyImage>();
                if (dto.ImageFiles != null && dto.ImageFiles.Any())
                {
                    var savedPaths = await _fileManager.SaveManyAsync(dto.ImageFiles, "Properties");
                    foreach (var path in savedPaths.Files)
                    {
                        images.Add(new PropertyImage
                        {
                            PropertyId = property.Id,
                            ImageUrl = path,
                            CreateAt = DateTimeOffset.UtcNow,
                            UpdateAt = DateTimeOffset.UtcNow
                        });
                    }
                }
                property.Images = images;

                await _propertyRepository.AddAsync(property);
                var result = await _propertyRepository.SaveAsync();
                if (result > 0)
                {
                    return ValidationResult.Success();
                }

                return ValidationResult.Failure(new Error("Oops", "Ocurrió un error al procesar la solicitud. Favor inténtelo de nuevo más tarde."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public override async Task<ValidationResult> RemoveAsync(int id)
        {
            try
            {
                var validation = await _validationService.ValidateForDeleteAsync(id);
                if (!validation.IsValid)
                {
                    return validation;
                }

                var property = await _propertyRepository.GetByIdWithImagesAsync(id);
                if (property != null && property.Images != null && property.Images.Any())
                {
                    var ids = property.Images
                        .Select(img => System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(img.ImageUrl)))
                        .Where(name => !string.IsNullOrEmpty(name))
                        .ToList();
                    await _fileManager.DeleteManyAsync(ids!, "Properties");
                }

                return await base.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableAsync(PropertyFilterDto? filters)
        {
            try
            {
                IReadOnlyCollection<Property> properties;

                if (filters != null && (filters.MinPrice.HasValue || filters.MaxPrice.HasValue || filters.Bedrooms.HasValue || filters.Bathrooms.HasValue))
                {
                    var criteria = _mapper.Map<PropertyFilterCriteria>(filters);
                    properties = await _propertyRepository.GetFilteredPropertiesAsync(criteria);
                }
                else
                {
                    properties = await _propertyRepository.GetAvailablePropertiesAsync();
                }

                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyDto>> GetByCodeAsync(string code)
        {
            try
            {
                var property = await _propertyRepository.GetAvailablePropertyByCodeAsync(code);
                if (property == null)
                {
                    return ValidationResult<PropertyDto>.Failure(Domain.Common.CodeErrors.Property.PropertyErrors.NotFoundByCode);
                }
                var dto = _mapper.Map<PropertyDto>(property);
                return ValidationResult<PropertyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                var property = await _propertyRepository.GetByIdWithImagesAsync(id);
                if (property == null)
                {
                    return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Property.NotFound", "La propiedad no existe.") });
                }
                var dto = _mapper.Map<PropertyDto>(property);
                return ValidationResult<PropertyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAvailableByAgentAsync(string agentId)
        {
            try
            {
                var properties = await _propertyRepository.GetAvailablePropertiesByAgentAsync(agentId);
                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetPropertiesByAgentAsync(string agentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var properties = await _propertyRepository.GetPropertiesByAgentAsync(agentId, pageNumber, pageSize);
                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<PropertyDto>>> GetAllWithDetailsAsync()
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var dtos = _mapper.Map<IReadOnlyCollection<PropertyDto>>(properties);
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<IReadOnlyCollection<PropertyDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyTotalsDto>> GetTotalsByStatusAsync()
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var totals = new PropertyTotalsDto
                {
                    AvailableProperties = properties.Count(p => p.Status == PropertyState.Available),
                    SoldProperties = properties.Count(p => p.Status == PropertyState.Sold)
                };
                return ValidationResult<PropertyTotalsDto>.Success(totals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<PropertyTotalsDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<int>> CountByAgentAsync(string agentId)
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync();
                var count = properties.Count(p => p.AgentId == agentId);
                return ValidationResult<int>.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public override async Task<ValidationResult?> UpdateAsync(SavePropertyDto dto)
        {
            try
            {
                var validation = await _validationService.ValidateForUpdateAsync(dto);
                if (!validation.IsValid)
                {
                    return validation;
                }

                var property = await _propertyRepository.GetByIdWithImagesAsync(dto.Id);
                if (property == null)
                {
                    return ValidationResult.Failure(new Error("Property.NotFound", "La propiedad a actualizar no existe."));
                }

                var newImagesCount = dto.ImageFiles?.Count ?? 0;
                var keptImagesCount = dto.ExistingImageUrls?.Count ?? 0;
                if (newImagesCount + keptImagesCount == 0)
                {
                    return ValidationResult.Failure(new Error("Property.NoImages", "Debe mantener al menos una imagen activa para la propiedad."));
                }

                property.Price = dto.Price;
                property.Description = dto.Description;
                property.Size = dto.Size;
                property.Bedrooms = dto.Bedrooms;
                property.Bathrooms = dto.Bathrooms;
                property.PropertyTypeId = dto.PropertyTypeId;
                property.SaleTypeId = dto.SaleTypeId;
                property.UpdateAt = DateTimeOffset.UtcNow;

                var currentImprovements = property.PropertyImprovements?.ToList() ?? new List<PropertyImprovement>();
                var toRemove = currentImprovements
                    .Where(pi => !dto.ImprovementIds.Contains(pi.ImprovementId))
                    .ToList();
                foreach (var relation in toRemove)
                {
                    await _propertyImprovementRepository.DeleteAsync(relation);
                }

                var currentIds = currentImprovements.Select(pi => pi.ImprovementId).ToList();
                var toAdd = dto.ImprovementIds.Except(currentIds);
                foreach (var improvementId in toAdd)
                {
                    await _propertyImprovementRepository.AddAsync(new PropertyImprovement
                    {
                        PropertyId = property.Id,
                        ImprovementId = improvementId
                    });
                }

                var currentImages = property.Images.ToList();
                var urlsToKeep = dto.ExistingImageUrls ?? new List<string>();

                var urlsToRemove = currentImages
                    .Select(img => img.ImageUrl)
                    .Where(url => !urlsToKeep.Contains(url))
                    .ToList();

                if (urlsToRemove.Any())
                {
                    var idsToRemove = urlsToRemove
                        .Select(url => System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(url)))
                        .Where(name => !string.IsNullOrEmpty(name))
                        .ToList();
                    await _fileManager.DeleteManyAsync(idsToRemove!, "Properties");
                }

                var updatedImagesList = currentImages.Where(img => urlsToKeep.Contains(img.ImageUrl)).ToList();
                if (dto.ImageFiles != null && dto.ImageFiles.Any())
                {
                    var savedPaths = await _fileManager.SaveManyAsync(dto.ImageFiles, "Properties");
                    
                    var newPropertyImages = savedPaths.Files.Select(path => new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImageUrl = path,
                        CreateAt = DateTimeOffset.UtcNow,
                        UpdateAt = DateTimeOffset.UtcNow
                    });
                    
                    updatedImagesList.AddRange(newPropertyImages);
                }
                property.Images = updatedImagesList;

                await _propertyRepository.UpdateAsync(property);
                return ValidationResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento."));
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
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
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento."));
            }
        }

        public async Task<bool> IsAvailableAsync(int propertyId)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(propertyId);
                return property != null && property.Status == PropertyState.Available;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en PropertyService");
                return false;
            }
        }
    }
}
