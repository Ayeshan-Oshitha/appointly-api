using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IModelRepository _modelRepository;

        public BrandService(IBrandRepository brandRepository, IModelRepository modelRepository)
        {
            _brandRepository = brandRepository;
            _modelRepository = modelRepository;
        }

        public async Task<Brand> AddBrand(string name)
        {
            var brand = new Brand
            {
                Name = name,
                Slug = name.Trim().ToLower().Replace(" ", "-")
            };

            if (await _brandRepository.BrandSlugExistsAsync(brand.Slug))
            {
                throw new ConflictException("Brand with the same name already exists.");
            }

            var addedBrand =  await _brandRepository.AddBrandAsync(brand);
            return addedBrand;
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrandsAsync();
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
                throw new Exception("Failed to delete the Brand");
            }
        }

        public async Task<Brand> UpdateBrand(Guid brandId, string name)
        {
            var existingBrand = await _brandRepository.GetBrandByIdAsync(brandId);

            if (existingBrand == null)
            {
                throw new NotFoundException("Brand not found"); 
            }

            existingBrand.Name = name;
            existingBrand.Slug = name.Trim().ToLower().Replace(" ", "-");
            await _brandRepository.SaveChangesAsync();
            return existingBrand;
        }
    }
}
