using MotorHub.Application.DTOs.Advertisements;

namespace MotorHub.Application.Services.Advertisements
{
    public interface IAdvertisementService
    {
        public Task<AdvertisementResponseDto> AddAdvertisement(CreateAdvertisementRequestDto request);
        public Task<List<AdvertisementDetailResponseDto>> GetAllAdvertisements(AdvertisementQueryDto query);
        public Task<AdvertisementResponseDto> UpdateAdvertisement(Guid id, UpdateAdvertisementRequestDto request);
        public Task DeleteAdvertisement(Guid id);
    }
}
