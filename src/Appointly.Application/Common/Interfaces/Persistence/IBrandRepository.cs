using Appointly.Domain.Entities;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IBrandRepository
    {
        public Task<List<Brand>> GetAllBrandsAsync();
        public Task<Brand?> GetBrandByIdAsync(Guid brandId);
        public Task<Brand> AddBrandAsync(Brand brand);
        public Task SaveChangesAsync();
        public Task<bool> DeleteBrandAsync(Guid brandId);
    }
}
