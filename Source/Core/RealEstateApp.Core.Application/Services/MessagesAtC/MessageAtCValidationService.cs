using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.CodeErrors.Message;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services.MessagesAtC
{
    public sealed class MessageAtCValidationService : IMessageAtCValidationService
    {
        private readonly IPropertyService _propertyService;

        public MessageAtCValidationService(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveMessageAtCDto dto)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                errors.Add(MessageErrors.EmptyMessage);
                return ValidationResult.Failure(errors);
            }

            var isAvailable = await _propertyService.IsAvailableAsync(dto.PropertyId);
            if (!isAvailable)
            {
                errors.Add(new Error("Message.PropertyNotAvailable", "La propiedad de la conversación no existe o no está disponible."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
