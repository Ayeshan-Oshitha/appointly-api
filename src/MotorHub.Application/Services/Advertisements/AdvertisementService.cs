using MotorHub.Application.Common.CurrentUser;
using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Advertisements;
using MotorHub.Domain.Common.Enum;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace MotorHub.Application.Services.Advertisements
{
    public class AdvertisementService : IAdvertisementService
    {

        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IModelRepository _modelRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        public AdvertisementService(IAdvertisementRepository advertisementRepository, IModelRepository modelRepository, ILocationRepository locationRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _advertisementRepository = advertisementRepository;
            _modelRepository = modelRepository;
            _locationRepository = locationRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<AdvertisementResponseDto> AddAdvertisement(CreateAdvertisementRequestDto request)
        {

            await ValidateBrandModelRelationship(request.ModelId, request.BrandId);
            await ValidateCityExists(request.CityId);

            var advertisement = new Advertisement
            {
                Title = request.Title,
                Description = request.Description,
                Year = request.Year,
                Price = request.Price,
                EngineCapacity = request.EngineCapacity,
                FuelType = request.FuelType!.Value,
                TransmissionType = request.TransmissionType!.Value,
                VehicleCondition = request.VehicleCondition!.Value,
                Address = request.Address,
                BrandId = request.BrandId,
                ModelId = request.ModelId,
                CityId = request.CityId,
                ContactName = request.ContactName,
                ContactPhone = request.ContactPhone,
                ContactEmail = request.ContactEmail,
                SellerId = _currentUser.Id,
                IsHidePhone = request.IsHidePhone,
                IsWhatsapp = request.IsWhatsapp,
                IsBiddable = request.IsBiddable
            };

           var addedAdvertisement = await _advertisementRepository.AddAdvertisementAsync(advertisement);
           return _mapper.Map<AdvertisementResponseDto>(addedAdvertisement);
        }

        public async  Task<List<AdvertisementDetailResponseDto>> GetAllAdvertisements(AdvertisementQueryDto query)
        {
            var isAdmin = _currentUser.IsAuthenticated && _currentUser.IsAdmin();
            Guid? sellerScopeId = _currentUser.IsAuthenticated && !isAdmin ? _currentUser.Id : null;

            var advertisements = await _advertisementRepository.GetAllAdvertisementsAsync(query, sellerScopeId, isAdmin);
            return _mapper.Map<List<AdvertisementDetailResponseDto>>(advertisements);
        }

        public async Task<AdvertisementResponseDto> UpdateAdvertisement(Guid id, UpdateAdvertisementRequestDto request)
        {
            var existingAdvertisement = await _advertisementRepository.GetAdvertisementByIdAsync(id);

            if (existingAdvertisement == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            EnsureCanModify(existingAdvertisement);

            // A PATCH can move either half of the pair, so validate the resulting combination
            // rather than only what the request happened to include. Skipping this let an
            // update write a Brand/Model pairing that create would have rejected.
            var newBrandId = request.BrandId ?? existingAdvertisement.BrandId;
            var newModelId = request.ModelId ?? existingAdvertisement.ModelId;

            if (newBrandId != existingAdvertisement.BrandId || newModelId != existingAdvertisement.ModelId)
            {
                await ValidateBrandModelRelationship(newModelId, newBrandId);
            }

            if (request.CityId.HasValue && request.CityId.Value != existingAdvertisement.CityId)
            {
                await ValidateCityExists(request.CityId.Value);
            }

            _mapper.Map(request, existingAdvertisement);

            existingAdvertisement.UpdatedAt = DateTime.UtcNow;

            // Edited content has to be reviewed again, otherwise a seller can get a bland ad
            // approved and then rewrite it into something that would never have passed.
            existingAdvertisement.Status = AdStatus.Pending;
            existingAdvertisement.RejectedReason = null;
            existingAdvertisement.ReviewByAdminId = null;
            existingAdvertisement.ReviewedAt = null;

            await _advertisementRepository.SaveAdvertisementAsync();
            return _mapper.Map<AdvertisementResponseDto>(existingAdvertisement);
        }

        public async Task DeleteAdvertisement(Guid id)
        {
            var existingAd = await _advertisementRepository.GetAdvertisementByIdAsync(id);

            if(existingAd == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            EnsureCanModify(existingAd);

            var deleted = await _advertisementRepository.DeleteAdvertisementAsync(id);

            if (!deleted)
            {
                throw new NotFoundException("Failed to delete the advertisement.");
            }
        }


        private void EnsureCanModify(Advertisement advertisement)
        {
            if (advertisement.SellerId != _currentUser.Id && !_currentUser.IsAdmin())
            {
                throw new ForbiddenException("You can only modify your own advertisements.");
            }
        }


        private async Task ValidateBrandModelRelationship(Guid modelId, Guid brandId)
        {
            var model = await _modelRepository.GetModelByIdAsync(modelId);

            if (model == null)
            {
                throw new NotFoundException("Model does not exist.");
            }

            if (model.BrandId != brandId)
            {
                throw new BadRequestException("Model does not belong to the specified brand.");
            }
        }


        // Checked up front so an unknown city is a 404 rather than a raw FK violation surfacing
        // as a 500 from SaveChanges.
        private async Task ValidateCityExists(Guid cityId)
        {
            var city = await _locationRepository.GetCityById(cityId);

            if (city == null)
            {
                throw new NotFoundException("City does not exist.");
            }
        }
    }
}
