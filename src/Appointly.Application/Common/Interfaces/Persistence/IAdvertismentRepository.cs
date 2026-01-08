using Appointly.Application.Services.Advertisments.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IAdvertismentRepository
    {
        Task<Advertisement> AddAdvertismentAsync(Advertisement advertisment);
        Task<Advertisement?> GetAdvertismentByIdAsync(Guid id);
        Task<List<Advertisement>> GetAllAdvertismentsAsync(AdvertisementQuery query);
        Task<bool> DeleteAdvertisementAsync(Guid id);
        public Task SaveAdvertisementAsync();
    }
}
