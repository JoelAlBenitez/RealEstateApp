using RealEstateApp.Core.Application.Contracts.Message;
using RealEstateApp.Core.Application.DTOs.MessageAtC;
using RealEstateApp.Core.Domain.Common.CodeErrors.Message;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Services.MessageAtC
{
    public sealed class MessageAtCValidationService : IMessageAtCValidationService
    {
        private readonly IPropertyRepository _propertyRepository;

        public MessageAtCValidationService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveMessageAtCDto dto)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                errors.Add(MessageErrors.EmptyMessage);
                return ValidationResult.Failure(errors);
            }

            var property = await _propertyRepository.GetByIdAsync(dto.PropertyId);
            if (property == null)
            {
                errors.Add(new Error("Message.PropertyNotFound", "La propiedad de la conversación no existe"));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
