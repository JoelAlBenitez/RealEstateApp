using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Domain.Common.CodeErrors.Offer;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Common.Errors;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;

namespace RealEstateApp.Core.Application.Services.Offers
{
    public sealed class OfferValidationService : IOfferValidationService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IPropertyService _propertyService;
        private readonly IUserSession _userSession;

        public OfferValidationService(
            IOfferRepository offerRepository, 
            IPropertyService propertyService,
            IUserSession userSession)
        {
            _offerRepository = offerRepository;
            _propertyService = propertyService;
            _userSession = userSession;
        }

        public async Task<ValidationResult> ValidateForCreateAsync(SaveOfferDto dto)
        {
            var errors = new List<Error>();

            if (dto.Amount <= 0)
            {
                errors.Add(OfferErrors.InvalidAmount);
                return ValidationResult.Failure(errors);
            }

            var isAvailable = await _propertyService.IsAvailableAsync(dto.PropertyId);
            if (!isAvailable)
            {
                errors.Add(new Error("Offer.PropertyNotFound", "La propiedad especificada no existe o no está disponible."));
                return ValidationResult.Failure(errors);
            }

            var otherOffers = await _offerRepository.GetPendingOffersByPropertyAsync(dto.PropertyId);
            if (otherOffers.Any(o => o.Status == OfferState.Accepted))
            {
                errors.Add(OfferErrors.PropertyHasAcceptedOffer);
                return ValidationResult.Failure(errors);
            }

            if (!string.IsNullOrEmpty(dto.CustomerId))
            {
                var pendingOffer = await _offerRepository.GetPendingOfferByClientAndPropertyAsync(dto.CustomerId, dto.PropertyId);
                if (pendingOffer != null)
                {
                    errors.Add(OfferErrors.PendingOfferExists);
                }
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForAcceptAsync(int offerId)
        {
            var errors = new List<Error>();
            var agentId = _userSession.GetIdCurrentUser();

            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null)
            {
                errors.Add(new Error("Offer.NotFound", "La oferta no existe."));
                return ValidationResult.Failure(errors);
            }

            if (offer.Status != OfferState.Pending)
            {
                errors.Add(OfferErrors.OfferAlreadyAnswered);
                return ValidationResult.Failure(errors);
            }

            var propertyResult = await _propertyService.GetByIdAsync(offer.PropertyId);
            if (!propertyResult.IsValid || propertyResult.Value == null)
            {
                errors.Add(new Error("Offer.PropertyNotFound", "La propiedad asociada a la oferta no existe."));
                return ValidationResult.Failure(errors);
            }
            var property = propertyResult.Value;

            if (property.Status == PropertyState.Sold)
            {
                errors.Add(OfferErrors.PropertyAlreadySold);
            }

            if (property.AgentId != agentId)
            {
                errors.Add(new Error("Offer.UnauthorizedAgent", "No tiene permisos para gestionar ofertas en esta propiedad."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForRejectAsync(int offerId)
        {
            var errors = new List<Error>();
            var agentId = _userSession.GetIdCurrentUser();

            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null)
            {
                errors.Add(new Error("Offer.NotFound", "La oferta no existe."));
                return ValidationResult.Failure(errors);
            }

            if (offer.Status != OfferState.Pending)
            {
                errors.Add(OfferErrors.OfferAlreadyAnswered);
                return ValidationResult.Failure(errors);
            }

            var propertyResult = await _propertyService.GetByIdAsync(offer.PropertyId);
            if (!propertyResult.IsValid || propertyResult.Value == null || propertyResult.Value.AgentId != agentId)
            {
                errors.Add(new Error("Offer.UnauthorizedAgent", "No tiene permisos para gestionar ofertas en esta propiedad."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateForCancelAsync(int offerId)
        {
            var errors = new List<Error>();
            var customerId = _userSession.GetIdCurrentUser();

            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null)
            {
                errors.Add(new Error("Offer.NotFound", "La oferta no existe."));
                return ValidationResult.Failure(errors);
            }

            if (offer.Status != OfferState.Pending)
            {
                errors.Add(OfferErrors.OfferAlreadyAnswered);
            }

            if (offer.CustomerId != customerId)
            {
                errors.Add(new Error("Offer.UnauthorizedCustomer", "No tiene permisos para cancelar esta oferta."));
            }

            return errors.Count > 0 ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
