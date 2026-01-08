using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Brands
{
    public interface IBrandService
    {
        public Task<List<Brand>> GetAllBrands();
        public Task<Brand> AddBrand(string name);
        public Task<Brand> UpdateBrand(Guid brandId, string name);
        public Task DeleteBrand(Guid brandId);
    }
}
