using MotorHub.Application.DTOs.Advertisements;
using MotorHub.Domain.Entities;

namespace MotorHub.Application.Common.Interfaces.Persistence
{
    public interface IAdvertisementRepository
    {
        Task<Advertisement> AddAdvertisementAsync(Advertisement advertisement);
        Task<Advertisement?> GetAdvertisementByIdAsync(Guid id);
        Task<List<Advertisement>> GetAllAdvertisementsAsync(AdvertisementQueryDto query, Guid? sellerScopeId, bool isAdmin);
        Task<bool> DeleteAdvertisementAsync(Guid id);
        public Task SaveAdvertisementAsync();
        
    }
}
