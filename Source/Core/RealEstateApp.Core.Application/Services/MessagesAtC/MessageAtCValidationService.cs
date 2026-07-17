using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.CodeErrors.Message;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Core.Application.Services.MessagesAtC
{
    public sealed class MessageAtCValidationService : IMessageAtCValidationService
    {
        private readonly IPropertyService _propertyService;
        private readonly IUserSession _userSession;

        public MessageAtCValidationService(IPropertyService propertyService, IUserSession userSession)
        {
            _propertyService = propertyService;
            _userSession = userSession;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveMessageAtCDto dto)
        {
            var errors = new List<Error>();

            var roles = _userSession.GetRolesCurrentUser();
            if (!roles.Contains(Roles.Cliente.ToString()) && !roles.Contains(Roles.Agente.ToString()))
            {
                errors.Add(new Error("Mensaje.UsuarioNoAutorizado", "Solo los clientes y agentes pueden enviar mensajes."));
                return ValidationResult.Failure(errors);
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                errors.Add(MessageErrors.EmptyMessage);
                return ValidationResult.Failure(errors);
            }

            var isAvailable = await _propertyService.IsAvailableAsync(dto.PropertyId);
            if (!isAvailable)
            {
                errors.Add(new Error("Mensaje.PropiedadNoDisponible", "La propiedad de la conversación no existe o no está disponible."));
            }

            if (roles.Contains(Roles.Agente.ToString()))
            {
                var propertyResult = await _propertyService.GetByIdAsync(dto.PropertyId);
                if (!propertyResult.IsValid || propertyResult.Value == null || propertyResult.Value.AgentId != _userSession.GetIdCurrentUser())
                {
                    errors.Add(new Error("Mensaje.AgenteNoAutorizado", "No tienes permisos para enviar mensajes relacionados a esta propiedad."));
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
