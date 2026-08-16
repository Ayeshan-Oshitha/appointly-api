using MotorHub.Application.DTOs.Brand;

namespace MotorHub.Application.Services.Brands
{
    public interface IBrandService
    {
        public Task<List<BrandResponseDto>> GetAllBrands();
        public Task<BrandResponseDto> AddBrand(CreateBrandRequestDto request);
        public Task<BrandResponseDto> UpdateBrand(Guid brandId, UpdateBrandRequestDto request);
        public Task DeleteBrand(Guid brandId);
    }
}
