using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Models
{
    public interface IModelService
    {
        public Task<List<Model>> GetModels(Guid? brandId);
        public Task<Model> AddModel(string name, Guid brandId);
        public Task<Model> UpdateModel(Guid modelId, string? name, Guid? brandId);
        public Task DeleteModel(Guid modelId);
    }
}
