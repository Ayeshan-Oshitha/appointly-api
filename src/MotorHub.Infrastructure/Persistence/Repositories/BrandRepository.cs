using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MotorHub.Infrastructure.Persistence.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly MotorHubDbContext _dbContext;

        public BrandRepository(MotorHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesOrConflictAsync("Brand with the same name already exists.");
            return brand;
        }

        public async Task<bool> DeleteBrandAsync(Guid brandId)
        {
            var existingBrand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == brandId);
            if (existingBrand == null)
            {
                return false;
            }

            _dbContext.Brands.Remove(existingBrand);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _dbContext.Brands.AsNoTracking().ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(Guid brandId)
        {
            return await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == brandId);
        }

        public Task<bool> BrandSlugExistsAsync(string slug, Guid? excludeBrandId = null)
        {
            return _dbContext.Brands.AnyAsync(b => b.Slug == slug
                && (!excludeBrandId.HasValue || b.Id != excludeBrandId.Value));
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesOrConflictAsync("Brand with the same name already exists.");
        }
    }
}
