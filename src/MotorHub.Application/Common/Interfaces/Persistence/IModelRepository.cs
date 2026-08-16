using MotorHub.Domain.Entities;

namespace MotorHub.Application.Common.Interfaces.Persistence
{
    public interface IModelRepository
    {
        public Task<List<Model>> GetAllModels(Guid? brandId);
        public Task<Model?> GetModelByIdAsync(Guid modelId);

        // Untracked read with Brand included; see ILocationRepository.GetCityDetailAsync.
        public Task<Model?> GetModelDetailAsync(Guid modelId);
        public Task<Model> AddModelAsync(Model model);
        public Task SaveModelAsync();
        public Task<bool> DeleteModelAsync(Guid modelId);
        // See IBrandRepository.BrandSlugExistsAsync for why the exclusion exists.
        Task<bool> ModelSlugExistsAsync(string slug, Guid? excludeModelId = null);
        Task<bool> HasModelsAsync(Guid brandId);
    }
}
