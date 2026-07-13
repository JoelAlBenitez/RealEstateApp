using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyValidationService : IPropertyValidationService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyValidationService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SavePropertyDto dto)
        {
            var errors = new List<Error>();

            if (dto.Price <= 0)
                errors.Add(new Error("Property.InvalidPrice", "El precio de la propiedad debe ser mayor a cero."));

            if (dto.Size <= 0)
                errors.Add(new Error("Property.InvalidSize", "El tamaño de la propiedad debe ser mayor a cero."));

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add(new Error("Property.InvalidDescription", "La descripción de la propiedad es requerida."));

            await Task.CompletedTask;

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForDeleteAsync(int id)
        {
            var errors = new List<Error>();

            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
            {
                errors.Add(new Error("Property.NotFound", "La propiedad a eliminar no existe."));
                return ValidationResult.Failure(errors);
            }

            if (property.Status == PropertyState.Sold)
            {
                errors.Add(new Error("Property.CannotDeleteSold", "No se puede eliminar una propiedad que ya ha sido vendida."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(SavePropertyDto dto)
        {
            var errors = new List<Error>();

            var property = await _propertyRepository.GetByIdAsync(dto.Id);
            if (property == null)
            {
                errors.Add(new Error("Property.NotFound", "La propiedad a actualizar no existe."));
                return ValidationResult.Failure(errors);
            }

            if (dto.Price <= 0)
                errors.Add(new Error("Property.InvalidPrice", "El precio de la propiedad debe ser mayor a cero."));

            if (dto.Size <= 0)
                errors.Add(new Error("Property.InvalidSize", "El tamaño de la propiedad debe ser mayor a cero."));

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add(new Error("Property.InvalidDescription", "La descripción de la propiedad es requerida."));

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
