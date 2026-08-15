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
            return _mapper.Map<ModelResponseDto>(addedModel);
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
                existingModel.Name = request.Name;
                existingModel.Slug = request.Name.Trim().ToLower().Replace(" ", "-");
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
            return _mapper.Map<ModelResponseDto>(existingModel);
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
