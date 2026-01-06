using Appointly.Application.Services.Advertisment.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Advertisements
{
    public interface IAdvertisementService
    {
        public Task<Advertisement> AddAdvertisment(CreateAdvertisementRequest request);
        public Task<List<Advertisement>> GetAllAdvertisments();
    }
}
