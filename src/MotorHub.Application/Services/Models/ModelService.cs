using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Model;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace MotorHub.Application.Services.Models
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        public ModelService(IModelRepository modelRepository, IBrandRepository brandRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        public async Task<ModelResponseDto> AddModel(CreateModelRequestDto request)
        {
            var existingBrandId = await _brandRepository.GetBrandByIdAsync(request.BrandId);

            if (existingBrandId == null)
            {
                throw new BadRequestException("Brand does not exist.");
            }

            var model = new Model
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Slug = request.Name.Trim().ToLower().Replace(" ", "-"),
                BrandId = request.BrandId
            };

            if (await _modelRepository.ModelSlugExistsAsync(model.Slug))
            {
                throw new ConflictException("Model with the same name already exists.");
            }

            var addedModel = await _modelRepository.AddModelAsync(model);

            // ModelResponseDto carries the brand *name*, which lives on a navigation property a
            // freshly-inserted entity has never loaded. Re-read through GetModelByIdAsync, which
            // Includes Brand, instead of mapping the value the write returned.
            return await MapModelWithBrandAsync(addedModel.Id);
        }

        public async Task<List<ModelResponseDto>> GetModels(Guid? brandId)
        {
            var models = await _modelRepository.GetAllModels(brandId);
            return _mapper.Map<List<ModelResponseDto>>(models);
        }

        public async Task<ModelResponseDto> UpdateModel(Guid modelId, UpdateModelRequestDto request)
        {
            var existingModel = await _modelRepository.GetModelByIdAsync(modelId);

            if (existingModel == null)
            {
                throw new NotFoundException("Model not found.");
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                var slug = request.Name.Trim().ToLower().Replace(" ", "-");

                // The create path checks this; without the same check here a rename onto an
                // existing model's name reaches the unique index and fails as a 500, not a 409.
                if (await _modelRepository.ModelSlugExistsAsync(slug, modelId))
                {
                    throw new ConflictException("Model with the same name already exists.");
                }

                existingModel.Name = request.Name;
                existingModel.Slug = slug;
            }

            if (request.BrandId.HasValue)
            {
                var existingBrandId = await _brandRepository.GetBrandByIdAsync(request.BrandId.Value);
                if (existingBrandId == null)
                {
                    throw new BadRequestException("Brand does not exist.");
                }
                existingModel.BrandId = request.BrandId.Value;
            }

            await _modelRepository.SaveModelAsync();

            // Changing BrandId does not necessarily move the Brand navigation with it, so mapping
            // existingModel here can pair the new BrandId with the old brand name.
            return await MapModelWithBrandAsync(existingModel.Id);
        }

        // Reads the model back through the loader that Includes Brand, so the response always has
        // the brand name ModelResponseDto expects.
        private async Task<ModelResponseDto> MapModelWithBrandAsync(Guid modelId)
        {
            var model = await _modelRepository.GetModelDetailAsync(modelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found.");
            }

            return _mapper.Map<ModelResponseDto>(model);
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
                throw new NotFoundException("Failed to delete the model.");  
            }
        }
    }
}
