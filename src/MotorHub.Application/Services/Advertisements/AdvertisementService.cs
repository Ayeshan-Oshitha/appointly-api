using MotorHub.Application.Common.CurrentUser;
using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Advertisements;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace MotorHub.Application.Services.Advertisements
{
    public class AdvertisementService : IAdvertisementService
    {

        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IModelRepository _modelRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        public AdvertisementService(IAdvertisementRepository advertisementRepository, IModelRepository modelRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _advertisementRepository = advertisementRepository;
            _modelRepository = modelRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<AdvertisementResponseDto> AddAdvertisement(CreateAdvertisementRequestDto request)
        {

            await ValidateBrandModelRelationship(request.ModelId, request.BrandId);

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
            var advertisements = await _advertisementRepository.GetAllAdvertisementsAsync(query);
            return _mapper.Map<List<AdvertisementDetailResponseDto>>(advertisements);
        }

        public async Task<AdvertisementResponseDto> UpdateAdvertisement(Guid id, UpdateAdvertisementRequestDto request)
        {
            var existingAdvertisement = await _advertisementRepository.GetAdvertisementByIdAsync(id);

            if (existingAdvertisement == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            _mapper.Map(request, existingAdvertisement);

            existingAdvertisement.UpdatedAt = DateTime.UtcNow;

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

            var deleted = await _advertisementRepository.DeleteAdvertisementAsync(id);

            if (!deleted)
            {
                throw new Exception("Failed to delete the advertisement.");
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
    }
}
