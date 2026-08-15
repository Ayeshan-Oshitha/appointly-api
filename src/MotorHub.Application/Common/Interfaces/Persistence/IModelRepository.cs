using MotorHub.Domain.Entities;

namespace MotorHub.Application.Common.Interfaces.Persistence
{
    public interface IModelRepository
    {
        public Task<List<Model>> GetAllModels(Guid? brandId);
        public Task<Model?> GetModelByIdAsync(Guid modelId);
        public Task<Model> AddModelAsync(Model model);
        public Task SaveModelAsync();
        public Task<bool> DeleteModelAsync(Guid modelId);
        Task<bool> ModelSlugExistsAsync(string slug);
        Task<bool> HasModelsAsync(Guid brandId);
    }
}
