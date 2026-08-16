using MotorHub.Domain.Entities;

namespace MotorHub.Application.Common.Interfaces.Persistence
{
    public interface IBrandRepository
    {
         Task<List<Brand>> GetAllBrandsAsync();
         Task<Brand?> GetBrandByIdAsync(Guid brandId);
         Task<Brand> AddBrandAsync(Brand brand);
         Task SaveChangesAsync();
         Task<bool> DeleteBrandAsync(Guid brandId);
         // excludeBrandId lets an update ask "is this slug taken by anyone *else*", so renaming a
         // brand without changing its name is not reported as a conflict with itself.
         Task<bool> BrandSlugExistsAsync(string slug, Guid? excludeBrandId = null);
    }
}
