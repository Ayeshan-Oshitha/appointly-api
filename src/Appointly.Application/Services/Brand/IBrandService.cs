using Appointly.Application.DTOs.Brand;

namespace Appointly.Application.Services.Brands
{
    public interface IBrandService
    {
        public Task<List<BrandResponseDto>> GetAllBrands();
        public Task<BrandResponseDto> AddBrand(CreateBrandRequestDto request);
        public Task<BrandResponseDto> UpdateBrand(Guid brandId, UpdateBrandRequestDto request);
        public Task DeleteBrand(Guid brandId);
    }
}
