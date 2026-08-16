using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Brand;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace MotorHub.Application.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IModelRepository modelRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<BrandResponseDto> AddBrand(CreateBrandRequestDto request)
        {
            var brand = new Brand
            {
                Name = request.Name,
                Slug = request.Name.Trim().ToLower().Replace(" ", "-")
            };

            if (await _brandRepository.BrandSlugExistsAsync(brand.Slug))
            {
                throw new ConflictException("Brand with the same name already exists.");
            }

            var addedBrand =  await _brandRepository.AddBrandAsync(brand);
            return _mapper.Map<BrandResponseDto>(addedBrand);
        }

        public async Task<List<BrandResponseDto>> GetAllBrands()
        {
            var brands = await _brandRepository.GetAllBrandsAsync();
            return _mapper.Map<List<BrandResponseDto>>(brands);
        }

        public async Task DeleteBrand(Guid brandId)
        {
            var existingBrand = await _brandRepository.GetBrandByIdAsync(brandId);

            if (existingBrand == null)
            {
                throw new NotFoundException("Brand not found");
            }

            var isModelsExisting = await _modelRepository.HasModelsAsync(brandId);

            if (isModelsExisting)
            {
                throw new ConflictException("This brand has existing Models. Please remove them first");
            }

            var deleted = await _brandRepository.DeleteBrandAsync(brandId);

            if (!deleted)
            {
                throw new NotFoundException("Failed to delete the Brand");
            }
        }

        public async Task<BrandResponseDto> UpdateBrand(Guid brandId, UpdateBrandRequestDto request)
        {
            var existingBrand = await _brandRepository.GetBrandByIdAsync(brandId);

            if (existingBrand == null)
            {
                throw new NotFoundException("Brand not found");
            }

            existingBrand.Name = request.Name;
            existingBrand.Slug = request.Name.Trim().ToLower().Replace(" ", "-");
            await _brandRepository.SaveChangesAsync();
            return _mapper.Map<BrandResponseDto>(existingBrand);
        }
    }
}
