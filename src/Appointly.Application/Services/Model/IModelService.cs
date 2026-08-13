using Appointly.Application.DTOs.Model;

namespace Appointly.Application.Services.Models
{
    public interface IModelService
    {
        public Task<List<ModelResponseDto>> GetModels(Guid? brandId);
        public Task<ModelResponseDto> AddModel(CreateModelRequestDto request);
        public Task<ModelResponseDto> UpdateModel(Guid modelId, UpdateModelRequestDto request);
        public Task DeleteModel(Guid modelId);
    }
}
