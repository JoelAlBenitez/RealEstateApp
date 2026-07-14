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

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyService : GenericServices<SavePropertyDto, Property, int>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyValidationService _validationService;
        private readonly IFileManager _fileManager;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IPropertyValidationService validationService,
            IMapper mapper,
            IFileManager fileManager)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _validationService = validationService;
            _fileManager = fileManager;
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
                    var existProperty = await _propertyRepository.GetAvailablePropertyByCodeAsync(code);
                    codeExists = existProperty != null;
                } while (codeExists);
                dto.Code = code;

                var property = _mapper.Map<Property>(dto);
                property.CreateAt = DateTimeOffset.UtcNow;
                property.UpdateAt = DateTimeOffset.UtcNow;

                var images = new List<PropertyImage>();
                if (dto.ImageFiles != null && dto.ImageFiles.Any())
                {
                    var savedPaths = await _fileManager.SaveManyAsync(dto.ImageFiles, "Properties");
                    foreach (var path in savedPaths)
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
            catch (Exception)
            {
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
                return await base.RemoveAsync(id);
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<PropertyDto>> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(id);
                if (property == null)
                {
                    return ValidationResult<PropertyDto>.Failure(new List<Error> { new Error("Property.NotFound", "La propiedad no existe.") });
                }
                var dto = _mapper.Map<PropertyDto>(property);
                return ValidationResult<PropertyDto>.Success(dto);
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult> DeletePropertiesByAgentAsync(string agentId)
        {
            try
            {
                await _propertyRepository.DeletePropertiesByAgentAsync(agentId);
                return ValidationResult.Success();
            }
            catch (Exception)
            {
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
                property.UpdateAt = DateTimeOffset.UtcNow;

                var currentImages = property.Images.ToList();
                var urlsToKeep = dto.ExistingImageUrls ?? new List<string>();

                var urlsToRemove = currentImages
                    .Select(img => img.ImageUrl)
                    .Where(url => !urlsToKeep.Contains(url))
                    .ToList();

                if (urlsToRemove.Any())
                {
                    await _fileManager.DeleteManyAsync(urlsToRemove, "Properties");
                }

                var updatedImagesList = currentImages.Where(img => urlsToKeep.Contains(img.ImageUrl)).ToList();
                if (dto.ImageFiles != null && dto.ImageFiles.Any())
                {
                    var savedPaths = await _fileManager.SaveManyAsync(dto.ImageFiles, "Properties");
                    
                    var newPropertyImages = savedPaths.Select(path => new PropertyImage
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
                var result = await _propertyRepository.SaveAsync();
                if (result > 0)
                {
                    return ValidationResult.Success();
                }

                return ValidationResult.Failure(new Error("Oops", "Ocurrió un error al procesar la solicitud."));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
            }
        }

        public async Task<ValidationResult<int>> CountByImprovementAsync(int improvementId)
        {
            try
            {
                return ValidationResult<int>.Success(0);
            }
            catch (Exception)
            {
                return ValidationResult<int>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento.") });
            }
        }

        public async Task<ValidationResult> DeleteByPropertyTypeAsync(int propertyTypeId)
        {
            try
            {
                await _propertyRepository.DeletePropertiesByPropertyTypeAsync(propertyTypeId);
                return ValidationResult.Success();
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento."));
            }
        }

        public async Task<ValidationResult> DeleteBySaleTypeAsync(int saleTypeId)
        {
            try
            {
                await _propertyRepository.DeletePropertiesBySaleTypeAsync(saleTypeId);
                return ValidationResult.Success();
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
