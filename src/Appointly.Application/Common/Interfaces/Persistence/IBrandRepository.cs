using Appointly.Domain.Entities;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IBrandRepository
    {
         Task<List<Brand>> GetAllBrandsAsync();
         Task<Brand?> GetBrandByIdAsync(Guid brandId);
         Task<Brand> AddBrandAsync(Brand brand);
         Task SaveChangesAsync();
         Task<bool> DeleteBrandAsync(Guid brandId);
         Task<bool> BrandSlugExistsAsync(string slug);
    }
}
