using Appointly.Api.Common.DTOs.Brands;
using Appointly.Application.Services.Brands;
using Appointly.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("brand")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrands();
            return Ok(_mapper.Map<List<BrandResponseDto>>(brands));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequestDto request)
        {
            var brand = await _brandService.AddBrand(request.Name);
            return Ok(_mapper.Map<BrandResponseDto>(brand));
        }

        [HttpPut("{brandId}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] Guid brandId, [FromBody] UpdateBrandRequestDto request)
        {
            var brand = await _brandService.UpdateBrand(brandId, request.Name);
            return Ok(_mapper.Map<BrandResponseDto>(brand));
        }

        [HttpDelete("{brandId}")]
        public async Task<IActionResult> DeleteBrand([FromRoute] Guid brandId)
        {
           await _brandService.DeleteBrand(brandId);
           return Ok("Brand Deleted Successfully");
        }
    }
}
