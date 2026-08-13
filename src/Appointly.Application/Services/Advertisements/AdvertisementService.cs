using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.DTOs.Advertisements;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace Appointly.Application.Services.Advertisements
{
    public class AdvertisementService : IAdvertisementService
    {

        private readonly IAdvertismentRepository _advertismentRepository;
        private readonly IModelRepository _modelRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        public AdvertisementService(IAdvertismentRepository advertismentRepository, IModelRepository modelRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _advertismentRepository = advertismentRepository;
            _modelRepository = modelRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<AdvertisementResponseDto> AddAdvertisment(CreateAdvertisementRequestDto request)
        {

            await ValidateBrandModelRelationship(request.ModelId, request.BrandId);

            var advertisment = new Advertisement
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

           var addedAdvertisment = await _advertismentRepository.AddAdvertismentAsync(advertisment);
           return _mapper.Map<AdvertisementResponseDto>(addedAdvertisment);
        }

        public async  Task<List<AdvertisementDetailResponseDto>> GetAllAdvertisments(AdvertisementQueryDto query)
        {
            var advertisments = await _advertismentRepository.GetAllAdvertismentsAsync(query);
            return _mapper.Map<List<AdvertisementDetailResponseDto>>(advertisments);
        }

        public async Task<AdvertisementResponseDto> UpdateAdvertisement(Guid id, UpdateAdvertisementRequestDto request)
        {
            var existingAdvertisement = await _advertismentRepository.GetAdvertismentByIdAsync(id);

            if (existingAdvertisement == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            _mapper.Map(request, existingAdvertisement);

            existingAdvertisement.UpdatedAt = DateTime.UtcNow;

            await _advertismentRepository.SaveAdvertisementAsync();
            return _mapper.Map<AdvertisementResponseDto>(existingAdvertisement);
        }

        public async Task DeleteAdvertisement(Guid id)
        {
            var existingAd = await _advertismentRepository.GetAdvertismentByIdAsync(id);

            if(existingAd == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            var deleted = await _advertismentRepository.DeleteAdvertisementAsync(id);

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
