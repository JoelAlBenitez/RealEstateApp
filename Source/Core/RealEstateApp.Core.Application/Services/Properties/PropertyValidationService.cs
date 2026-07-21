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

            if (dto.ImprovementIds == null || dto.ImprovementIds.Count == 0)
                errors.Add(new Error("Propiedad.MejorasRequeridas", "Debe seleccionar al menos una mejora para la propiedad."));

            if (dto.ImageFiles == null || dto.ImageFiles.Count == 0)
            {
                errors.Add(new Error("Propiedad.ImagenesRequeridas", "Debe cargar al menos una imagen de la propiedad."));
            }
            else if (dto.ImageFiles.Count > 4)
            {
                errors.Add(new Error("Propiedad.ExcesoImagenes", "Solo se permite registrar hasta 4 imágenes por propiedad."));
            }

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

            if (dto.Price <= 0)
                errors.Add(new Error("Propiedad.PrecioInvalido", "El precio de la propiedad debe ser mayor a cero."));

            if (dto.Size <= 0)
                errors.Add(new Error("Propiedad.TamanoInvalido", "El tamaño de la propiedad debe ser mayor a cero."));

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add(new Error("Propiedad.DescripcionInvalida", "La descripción de la propiedad es requerida."));

            if (dto.ImprovementIds == null || dto.ImprovementIds.Count == 0)
                errors.Add(new Error("Propiedad.MejorasRequeridas", "Debe seleccionar al menos una mejora para la propiedad."));

            var preExistingCount = dto.ExistingImageUrls?.Count ?? 0;
            var newCount = dto.ImageFiles?.Count ?? 0;
            var totalCount = preExistingCount + newCount;

            if (totalCount < 1)
            {
                errors.Add(new Error("Propiedad.ImagenesRequeridas", "Debe mantener o subir al menos una imagen para la propiedad."));
            }
            else if (totalCount > 4)
            {
                errors.Add(new Error("Propiedad.ExcesoImagenes", "Solo se permite un total de 4 imágenes por propiedad."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
