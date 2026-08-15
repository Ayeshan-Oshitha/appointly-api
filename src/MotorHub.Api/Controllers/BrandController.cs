using MotorHub.Application.DTOs.Brand;
using MotorHub.Application.Services.Brands;
using MotorHub.Domain.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MotorHub.Api.Controllers
{
    [Route("brand")]
    [ApiController]
    [Authorize(Roles = Roles.User)]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrands();
            return Ok(brands);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequestDto request)
        {
            var brand = await _brandService.AddBrand(request);
            return Ok(brand);
        }

        [HttpPut("{brandId:guid}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] Guid brandId, [FromBody] UpdateBrandRequestDto request)
        {
            var brand = await _brandService.UpdateBrand(brandId, request);
            return Ok(brand);
        }

        [HttpDelete("{brandId:guid}")]
        public async Task<IActionResult> DeleteBrand([FromRoute] Guid brandId)
        {
           await _brandService.DeleteBrand(brandId);
           return Ok("Brand Deleted Successfully");
        }
    }
}
