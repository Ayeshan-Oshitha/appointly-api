using Appointly.Application.Services.Advertisment.Contracts;
using Appointly.Application.Services.Advertisments.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Advertisements
{
    public interface IAdvertisementService
    {
        public Task<Advertisement> AddAdvertisment(CreateAdvertisementRequest request);
        public Task<List<Advertisement>> GetAllAdvertisments(AdvertisementQuery query);
        public Task<Advertisement> UpdateAdvertisement(Guid id, UpdateAdvertisementRequest request);
        public Task DeleteAdvertisement(Guid id);
    }
}
