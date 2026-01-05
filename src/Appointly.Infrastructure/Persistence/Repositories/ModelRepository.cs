using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly AppointlyDbContext _dbContext;
        public ModelRepository(AppointlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Model> AddModelAsync(Model model)
        {
            _dbContext.Models.Add(model);
            await _dbContext.SaveChangesAsync();
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
            return await _dbContext.Models.AsNoTracking().Include(m => m.Brand).FirstOrDefaultAsync(m => m.Id == modelId);
        }

        public  Task SaveModelAsync()
        {
            return  _dbContext.SaveChangesAsync();
        }
    }
}
