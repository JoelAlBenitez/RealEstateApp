using AutoMapper;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.Contracts.Offer;
using RealEstateApp.Core.Application.DTOs.Offer;
using RealEstateApp.Core.Domain.Common.ValidationResult;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;

namespace RealEstateApp.Core.Application.Services.Offer
{
    public sealed class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOfferValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly IGenericServices<SaveOfferDto, int> _genericService;

        public OfferService(
            IOfferRepository offerRepository,
            IPropertyRepository propertyRepository,
            IOfferValidationService validationService,
            IMapper mapper,
            IGenericServices<SaveOfferDto, int> genericService)
        {
            _offerRepository = offerRepository;
            _propertyRepository = propertyRepository;
            _validationService = validationService;
            _mapper = mapper;
            _genericService = genericService;
        }

        public async Task<ValidationResult> AddAsync(SaveOfferDto dto)
        {
            var validation = await _validationService.ValidateForCreateAsync(dto);
            if (!validation.IsValid)
            {
                return validation;
            }
            return await _genericService.AddAsync(dto);
        }

        public async Task<ValidationResult?> UpdateAsync(SaveOfferDto dto, int id)
        {
            return await _genericService.UpdateAsync(dto, id);
        }

        public async Task<ValidationResult<SaveOfferDto>> GetByIdAsync(int id)
        {
            return await _genericService.GetByIdAsync(id);
        }

        public async Task<ValidationResult<IReadOnlyCollection<SaveOfferDto>>> GetAllAsync()
        {
            return await _genericService.GetAllAsync();
        }

        public async Task<ValidationResult> DeleteAsync(int id)
        {
            return await _genericService.DeleteAsync(id);
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

            var rejectTasks = otherOffers.Select(o => {
                o.Status = OfferState.Rejected;
                return _offerRepository.UpdateAsync(o);
            });
            await Task.WhenAll(rejectTasks);

            var property = await _propertyRepository.GetByIdAsync(offer.PropertyId);
            property!.Status = PropertyState.Sold;
            await _propertyRepository.UpdateAsync(property);

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

            return ValidationResult.Success();
        }
    }
}
