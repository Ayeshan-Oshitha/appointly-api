using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Brand> AddBrand(string name)
        {
            var brand = new Brand
            {
                Name = name,
                Slug = name.Trim().ToLower().Replace(" ", "-")
            };

            var addedBrand =  await _brandRepository.AddBrandAsync(brand);
            return addedBrand;
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrandsAsync();
        }

        public async Task DeleteBrand(Guid brandId)
        {
            var existingBrand = await _brandRepository.DeleteBrandAsync(brandId);

            if (!existingBrand)
            {
                throw new NotFoundException("Brand not found");
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
