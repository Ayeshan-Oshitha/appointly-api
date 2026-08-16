using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            // The service checks the slug first, but another request can insert the same slug
            // between that check and this save. The unique index catches it either way; without
            // this the racing caller would get a 500 instead of the 409 the check produces.
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new ConflictException("Brand with the same name already exists.");
            }

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

        public Task<bool> BrandSlugExistsAsync(string slug)
        {
            return _dbContext.Brands.AnyAsync(b => b.Slug == slug);
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
