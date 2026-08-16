using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            await _dbContext.SaveChangesOrConflictAsync("Model with the same name already exists.");
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

        public Task<Model?> GetModelDetailAsync(Guid modelId)
        {
            return _dbContext.Models
                .AsNoTracking()
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == modelId);
        }

        public Task SaveModelAsync()
        {
            return _dbContext.SaveChangesOrConflictAsync("Model with the same name already exists.");
        }

        public Task<bool> ModelSlugExistsAsync(string slug, Guid? excludeModelId = null)
        {
            return _dbContext.Models.AnyAsync(m => m.Slug == slug
                && (!excludeModelId.HasValue || m.Id != excludeModelId.Value));
        }

        public async Task<bool> HasModelsAsync(Guid brandId)
        {
            return await _dbContext.Models
                .AnyAsync(m => m.BrandId == brandId);
        }
    }
}
