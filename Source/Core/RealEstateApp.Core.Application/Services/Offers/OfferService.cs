using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Core.Application.Services.Offers
{
    public sealed class OfferService : GenericServices<SaveOfferDto, Domain.Entities.Offer, int>, IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOfferValidationService _validationService;
        private readonly IUserSession _userSession;

        public OfferService(
            IOfferRepository offerRepository,
            IPropertyRepository propertyRepository,
            IOfferValidationService validationService,
            IMapper mapper,
            IUserSession userSession)
            : base(offerRepository, mapper)
        {
            _offerRepository = offerRepository;
            _propertyRepository = propertyRepository;
            _validationService = validationService;
            _userSession = userSession;
        }

        public override async Task<ValidationResult> AddAsync(SaveOfferDto dto)
        {
            try
            {
                dto.CustomerId = _userSession.GetIdCurrentUser();
                var validation = await _validationService.ValidateForCreateAsync(dto);
                if (!validation.IsValid)
                {
                    return validation;
                }
                return await base.AddAsync(dto);
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
                return await base.RemoveAsync(id);
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<OfferDto>>> GetPendingByPropertyAsync(int propertyId)
        {
            try
            {
                var offers = await _offerRepository.GetPendingOffersByPropertyAsync(propertyId);
                var dtos = _mapper.Map<IReadOnlyCollection<OfferDto>>(offers);
                return ValidationResult<IReadOnlyCollection<OfferDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<OfferDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult<IReadOnlyCollection<OfferDto>>> GetByCustomerAsync()
        {
            try
            {
                var customerId = _userSession.GetIdCurrentUser();
                var offers = await _offerRepository.GetOffersByClientAsync(customerId);
                var dtos = _mapper.Map<IReadOnlyCollection<OfferDto>>(offers);
                return ValidationResult<IReadOnlyCollection<OfferDto>>.Success(dtos);
            }
            catch (Exception)
            {
                return ValidationResult<IReadOnlyCollection<OfferDto>>.Failure(new List<Error> { new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde.") });
            }
        }

        public async Task<ValidationResult> AcceptOfferAsync(int offerId)
        {
            try
            {
                var validation = await _validationService.ValidateForAcceptAsync(offerId);
                if (!validation.IsValid)
                {
                    return validation;
                }

                var offer = await _offerRepository.GetByIdAsync(offerId);
                if (offer == null)
                {
                    return ValidationResult.Failure(new Error("Offer.NotFound", "La oferta especificada no existe."));
                }

                offer.Status = OfferState.Accepted;
                await _offerRepository.UpdateAsync(offer);
                await _offerRepository.RejectOtherOffersByPropertyAsync(offer.PropertyId, offerId);

                var property = await _propertyRepository.GetByIdAsync(offer.PropertyId);
                if (property == null)
                {
                    return ValidationResult.Failure(new Error("Property.NotFound", "La propiedad asociada a la oferta no existe."));
                }

                property.Status = PropertyState.Sold;
                await _propertyRepository.UpdateAsync(property);

                var result = await _offerRepository.SaveAsync();
                if (result <= 0)
                {
                    return ValidationResult.Failure(new Error("Oops", "Ocurrió un error al procesar la aceptación de la oferta. Inténtalo de nuevo más tarde."));
                }

                return ValidationResult.Success();
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult> RejectOfferAsync(int offerId)
        {
            try
            {
                var validation = await _validationService.ValidateForRejectAsync(offerId);
                if (!validation.IsValid)
                {
                    return validation;
                }

                var offer = await _offerRepository.GetByIdAsync(offerId);
                if (offer == null)
                {
                    return ValidationResult.Failure(new Error("Offer.NotFound", "La oferta especificada no existe."));
                }

                offer.Status = OfferState.Rejected;
                await _offerRepository.UpdateAsync(offer);

                var result = await _offerRepository.SaveAsync();
                if (result <= 0)
                {
                    return ValidationResult.Failure(new Error("Oops", "Ocurrió un error al rechazar la oferta. Inténtalo de nuevo más tarde."));
                }

                return ValidationResult.Success();
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }

        public async Task<ValidationResult> CancelOfferAsync(int offerId)
        {
            try
            {
                var validation = await _validationService.ValidateForCancelAsync(offerId);
                if (!validation.IsValid)
                {
                    return validation;
                }

                var offer = await _offerRepository.GetByIdAsync(offerId);
                if (offer == null)
                {
                    return ValidationResult.Failure(new Error("Offer.NotFound", "La oferta especificada no existe."));
                }

                await _offerRepository.DeleteAsync(offer);

                var result = await _offerRepository.SaveAsync();
                if (result <= 0)
                {
                    return ValidationResult.Failure(new Error("Oops", "Ocurrió un error al cancelar la oferta. Inténtalo de nuevo más tarde."));
                }

                return ValidationResult.Success();
            }
            catch (Exception)
            {
                return ValidationResult.Failure(new Error("Oops", "Al parecer esta función no está disponible en este momento. Favor intente más tarde."));
            }
        }
    }
}
