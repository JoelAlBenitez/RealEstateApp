using RealEstateApp.Core.Application.Contracts.Message;
using RealEstateApp.Core.Application.Contracts.Property;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.CodeErrors.Message;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;

namespace RealEstateApp.Core.Application.Services.MessageAtC
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

            var propertyResult = await _propertyService.GetByIdAsync(dto.PropertyId);
            if (!propertyResult.IsValid || propertyResult.Value == null)
            {
                errors.Add(new Error("Message.PropertyNotFound", "La propiedad de la conversación no existe"));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
