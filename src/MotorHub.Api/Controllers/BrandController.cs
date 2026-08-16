using MotorHub.Application.DTOs.Brand;
using MotorHub.Application.Services.Brands;
using MotorHub.Domain.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MotorHub.Api.Controllers
{
    [Route("brand")]
    [ApiController]
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
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequestDto request)
        {
            var brand = await _brandService.AddBrand(request);
            // 201 rather than CreatedAtAction: there is no GET-by-id action to point a Location
            // header at yet. Same for the other creates.
            return StatusCode(StatusCodes.Status201Created, brand);
        }

        [HttpPut("{brandId:guid}")]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> UpdateBrand([FromRoute] Guid brandId, [FromBody] UpdateBrandRequestDto request)
        {
            var brand = await _brandService.UpdateBrand(brandId, request);
            return Ok(brand);
        }

        [HttpDelete("{brandId:guid}")]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> DeleteBrand([FromRoute] Guid brandId)
        {
           await _brandService.DeleteBrand(brandId);
           return NoContent();
        }
    }
}
