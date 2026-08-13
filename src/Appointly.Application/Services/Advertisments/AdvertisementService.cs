using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Advertisements;
using Appointly.Application.Services.Advertisment.Contracts;
using Appointly.Application.Services.Advertisments.Contracts;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using Mapster;

namespace Appointly.Application.Services.Advertisments
{
    public class AdvertisementService : IAdvertisementService
    {

        private readonly IAdvertismentRepository _advertismentRepository;
        private readonly IModelRepository _modelRepository;
        private readonly ICurrentUser _currentUser;
        public AdvertisementService(IAdvertismentRepository advertismentRepository, IModelRepository modelRepository, ICurrentUser currentUser)
        {
            _advertismentRepository = advertismentRepository;
            _modelRepository = modelRepository;
            _currentUser = currentUser;
        }
        public async Task<Advertisement> AddAdvertisment(CreateAdvertisementRequest request)
        {

            await ValidateBrandModelRelationship(request.ModelId, request.BrandId);

            var advertisment = new Advertisement
            {
                Title = request.Title,
                Description = request.Description,
                Year = request.Year,
                Price = request.Price,
                EngineCapacity = request.EngineCapacity,
                FuelType = request.FuelType,
                TransmissionType = request.TransmissionType,
                VehicleCondition = request.VehicleCondition,
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
           return addedAdvertisment;
        }

        public async  Task<List<Advertisement>> GetAllAdvertisments(AdvertisementQuery query)
        {
            return await _advertismentRepository.GetAllAdvertismentsAsync(query);
        }

        public async Task<Advertisement> UpdateAdvertisement(Guid id, UpdateAdvertisementRequest request)
        {
            var existingAdvertisement = await _advertismentRepository.GetAdvertismentByIdAsync(id);

            if (existingAdvertisement == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            request.Adapt(existingAdvertisement);

            existingAdvertisement.UpdatedAt = DateTime.UtcNow;

            await _advertismentRepository.SaveAdvertisementAsync();
            return existingAdvertisement;
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
