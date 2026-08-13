using Appointly.Application.DTOs.Advertisements;

namespace Appointly.Application.Services.Advertisements
{
    public interface IAdvertisementService
    {
        public Task<AdvertisementResponseDto> AddAdvertisment(CreateAdvertisementRequestDto request);
        public Task<List<AdvertisementDetailResponseDto>> GetAllAdvertisments(AdvertisementQueryDto query);
        public Task<AdvertisementResponseDto> UpdateAdvertisement(Guid id, UpdateAdvertisementRequestDto request);
        public Task DeleteAdvertisement(Guid id);
    }
}
