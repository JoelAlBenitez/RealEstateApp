using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Core.Application.Services.Properties
{
    public sealed class PropertyValidationService : IPropertyValidationService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserSession _userSession;

        public PropertyValidationService(IPropertyRepository propertyRepository, IUserSession userSession)
        {
            _propertyRepository = propertyRepository;
            _userSession = userSession;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SavePropertyDto dto)
        {
            var errors = new List<Error>();

            var roles = _userSession.GetRolesCurrentUser();
            if (!roles.Contains(Roles.Agente.ToString()))
            {
                errors.Add(new Error("Propiedad.UsuarioNoAutorizado", "Solo los agentes pueden registrar propiedades."));
                return ValidationResult.Failure(errors);
            }

            if (dto.Price <= 0)
                errors.Add(new Error("Propiedad.PrecioInvalido", "El precio de la propiedad debe ser mayor a cero."));

            if (dto.Size <= 0)
                errors.Add(new Error("Propiedad.TamanoInvalido", "El tamaño de la propiedad debe ser mayor a cero."));

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add(new Error("Propiedad.DescripcionInvalida", "La descripción de la propiedad es requerida."));

            await Task.CompletedTask;

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForDeleteAsync(int id)
        {
            var errors = new List<Error>();

            var roles = _userSession.GetRolesCurrentUser();
            if (!roles.Contains(Roles.Agente.ToString()))
            {
                errors.Add(new Error("Propiedad.UsuarioNoAutorizado", "Solo los agentes pueden eliminar propiedades."));
                return ValidationResult.Failure(errors);
            }

            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
            {
                errors.Add(new Error("Propiedad.NoEncontrada", "La propiedad a eliminar no existe."));
                return ValidationResult.Failure(errors);
            }

            if (property.Status == PropertyState.Sold)
            {
                errors.Add(new Error("Propiedad.NoSePuedeEliminarVendida", "No se puede eliminar una propiedad que ya ha sido vendida."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForUpdateAsync(SavePropertyDto dto)
        {
            var errors = new List<Error>();

            var roles = _userSession.GetRolesCurrentUser();
            if (!roles.Contains(Roles.Agente.ToString()))
            {
                errors.Add(new Error("Propiedad.UsuarioNoAutorizado", "Solo los agentes pueden editar propiedades."));
                return ValidationResult.Failure(errors);
            }

            var property = await _propertyRepository.GetByIdAsync(dto.Id);
            if (property == null)
            {
                errors.Add(new Error("Propiedad.NoEncontrada", "La propiedad a actualizar no existe."));
                return ValidationResult.Failure(errors);
            }

            if (property.Status == PropertyState.Sold)
            {
                errors.Add(new Error("Propiedad.NoSePuedeEditarVendida", "No se puede editar una propiedad que ya ha sido vendida."));
            }

            if (property.Status == PropertyState.Sold)
            {
                errors.Add(new Error("Property.CannotUpdateSold", "No se puede editar una propiedad que ya ha sido vendida."));
            }

            if (dto.Price <= 0)
                errors.Add(new Error("Propiedad.PrecioInvalido", "El precio de la propiedad debe ser mayor a cero."));

            if (dto.Size <= 0)
                errors.Add(new Error("Propiedad.TamanoInvalido", "El tamaño de la propiedad debe ser mayor a cero."));

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add(new Error("Propiedad.DescripcionInvalida", "La descripción de la propiedad es requerida."));

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
