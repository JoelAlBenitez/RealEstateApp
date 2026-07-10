using AutoMapper;
using RealEstateApp.Core.Application.Contracts.Offer;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Application.Services.Generic;

namespace RealEstateApp.Core.Application.Services.Offer
{
    public sealed class OfferService : GenericServices<SaveOfferDto, Domain.Entities.Offer, int>, IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOfferValidationService _validationService;

        public OfferService(
            IOfferRepository offerRepository,
            IPropertyRepository propertyRepository,
            IOfferValidationService validationService,
            IMapper mapper)
            : base(offerRepository, mapper)
        {
            _offerRepository = offerRepository;
            _propertyRepository = propertyRepository;
            _validationService = validationService;
        }

        public override async Task<ValidationResult> AddAsync(SaveOfferDto dto)
        {
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await base.AddAsync(dto);
        }

        public override async Task<ValidationResult?> UpdateAsync(SaveOfferDto dto)
        {
            return await base.UpdateAsync(dto);
        }

        public async Task<ValidationResult<IReadOnlyCollection<OfferDto>>> GetPendingByPropertyAsync(int propertyId)
        {
            var offers = await _offerRepository.GetPendingOffersByPropertyAsync(propertyId);
            var dtos = _mapper.Map<IReadOnlyCollection<OfferDto>>(offers);
            return ValidationResult<IReadOnlyCollection<OfferDto>>.Success(dtos);
        }

        public async Task<ValidationResult<IReadOnlyCollection<OfferDto>>> GetByCustomerAsync(string customerId)
        {
            var offers = await _offerRepository.GetOffersByClientAsync(customerId);
            var dtos = _mapper.Map<IReadOnlyCollection<OfferDto>>(offers);
            return ValidationResult<IReadOnlyCollection<OfferDto>>.Success(dtos);
        }

        public async Task<ValidationResult> AcceptOfferAsync(int offerId, string agentId)
        {
            var validation = await _validationService.ValidateForAcceptAsync(offerId, agentId);
            if (!validation.IsValid)
            {
                return validation;
            }

            var offer = await _offerRepository.GetByIdAsync(offerId);
            offer!.Status = OfferState.Accepted;
            await _offerRepository.UpdateAsync(offer);

            var pendingOffers = await _offerRepository.GetPendingOffersByPropertyAsync(offer.PropertyId);
            var otherOffers = pendingOffers.Where(o => o.Id != offerId).ToList();

            foreach (var otherOffer in otherOffers)
            {
                otherOffer.Status = OfferState.Rejected;
                await _offerRepository.UpdateAsync(otherOffer);
            }

            var property = await _propertyRepository.GetByIdAsync(offer.PropertyId);
            property!.Status = PropertyState.Sold;
            await _propertyRepository.UpdateAsync(property);

            var result = await _offerRepository.SaveAsync();
            if (result <= 0)
            {
                return ValidationResult.Failure(new Domain.Common.Errors.Error("Oops", "Ocurrió un error al procesar la aceptación de la oferta. Inténtalo de nuevo más tarde."));
            }

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> RejectOfferAsync(int offerId, string agentId)
        {
            var validation = await _validationService.ValidateForRejectAsync(offerId, agentId);
            if (!validation.IsValid)
            {
                return validation;
            }

            var offer = await _offerRepository.GetByIdAsync(offerId);
            offer!.Status = OfferState.Rejected;
            await _offerRepository.UpdateAsync(offer);

            var result = await _offerRepository.SaveAsync();
            if (result <= 0)
            {
                return ValidationResult.Failure(new Domain.Common.Errors.Error("Oops", "Ocurrió un error al rechazar la oferta. Inténtalo de nuevo más tarde."));
            }

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> CancelOfferAsync(int offerId, string customerId)
        {
            var validation = await _validationService.ValidateForCancelAsync(offerId, customerId);
            if (!validation.IsValid)
            {
                return validation;
            }

            var offer = await _offerRepository.GetByIdAsync(offerId);
            await _offerRepository.DeleteAsync(offer!);

            var result = await _offerRepository.SaveAsync();
            if (result <= 0)
            {
                return ValidationResult.Failure(new Domain.Common.Errors.Error("Oops", "Ocurrió un error al cancelar la oferta. Inténtalo de nuevo más tarde."));
            }

            return ValidationResult.Success();
        }
    }
}
