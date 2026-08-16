using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MotorHub.Infrastructure.Persistence.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly MotorHubDbContext _dbContext;
        public ModelRepository(MotorHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Model> AddModelAsync(Model model)
        {
            _dbContext.Models.Add(model);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            // Slug uniqueness is checked in the service, but that check and this save are not
            // atomic; see the same guard in BrandRepository.AddBrandAsync.
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new ConflictException("Model with the same name already exists.");
            }

            return model;
        }

        public async Task<bool> DeleteModelAsync(Guid modelId)
        {
            var existingModel = await _dbContext.Models.FirstOrDefaultAsync(m => m.Id == modelId);
            if (existingModel == null)
            {
                return false;
            }

            _dbContext.Models.Remove(existingModel);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Model>> GetAllModels(Guid? brandId)
        {
            IQueryable<Model> query = _dbContext.Models.AsNoTracking().Include(m => m.Brand);

            if (brandId.HasValue)
            {
                query = query.Where(m => m.BrandId == brandId.Value);
            }

            return await query.OrderBy(m => m.Slug).ToListAsync();
        }

        public async Task<Model?> GetModelByIdAsync(Guid modelId)
        {
            return await _dbContext.Models.Include(m => m.Brand).FirstOrDefaultAsync(m => m.Id == modelId);
        }

        public  Task SaveModelAsync()
        {
            return  _dbContext.SaveChangesAsync();
        }

        public Task<bool> ModelSlugExistsAsync(string slug)
        {
            return _dbContext.Models.AnyAsync(c => c.Slug == slug);
        }

        public async Task<bool> HasModelsAsync(Guid brandId)
        {
            return await _dbContext.Models
                .AnyAsync(m => m.BrandId == brandId);
        }
    }
}
