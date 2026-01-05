using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.Models
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IBrandRepository _brandRepository;
        public ModelService(IModelRepository modelRepository, IBrandRepository brandRepository)
        {
            _modelRepository = modelRepository;
            _brandRepository = brandRepository;
        }
        public async Task<Model> AddModel(string name, Guid brandId)
        {
            var existingBrandId = await _brandRepository.GetBrandByIdAsync(brandId);

            if (existingBrandId == null)
            {
                throw new BadRequestException("Brand does not exist.");
            }

            var model = new Model
            {
                Id = Guid.NewGuid(),
                Name = name,
                Slug = name.Trim().ToLower().Replace(" ", "-"),
                BrandId = brandId
            };

            var addedModel = await _modelRepository.AddModelAsync(model);
            return addedModel;
        }

        public async Task<List<Model>> GetModels(Guid? brandId)
        {
            return await _modelRepository.GetAllModels(brandId);
        }

        public async Task<Model> UpdateModel(Guid modelId, string? name, Guid? brandId)
        {
            var existingModel = await _modelRepository.GetModelByIdAsync(modelId);

            if (existingModel == null)
            {
                throw new NotFoundException("Model not found.");
            }

            if (!string.IsNullOrEmpty(name))
            {
                existingModel.Name = name;
                existingModel.Slug = name.Trim().ToLower().Replace(" ", "-");
            }

            if (brandId.HasValue)
            {
                var existingBrandId = await _brandRepository.GetBrandByIdAsync(brandId.Value);
                if (existingBrandId == null)
                {
                    throw new BadRequestException("Brand does not exist.");
                }
                existingModel.BrandId = brandId.Value;
            }

            await _modelRepository.SaveModelAsync();
            return existingModel;
        }

        public async Task DeleteModel(Guid modelId)
        {
            var existingModel = await _modelRepository.GetModelByIdAsync(modelId);

            if (existingModel == null)
            {
                throw new NotFoundException("Model not found.");
            }

            var deleted = await _modelRepository.DeleteModelAsync(modelId);

            if (!deleted)
            {
                throw new Exception("Failed to delete the model.");  
            }
        }
    }
}
