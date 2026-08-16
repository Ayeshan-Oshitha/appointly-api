using MotorHub.Application.DTOs.Advertisements;
using MotorHub.Application.Services.Advertisements;
using MotorHub.Domain.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MotorHub.Api.Controllers
{
    [Route("advertisement")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAdvertisements([FromQuery] AdvertisementQueryDto query)
        {
            var advertisements = await _advertisementService.GetAllAdvertisements(query);
            return Ok(advertisements);
        }

        [HttpPost]
        [Authorize(Policy = Policies.SellerOnly)]
        public async Task<IActionResult> CreateAdvertisement([FromBody] CreateAdvertisementRequestDto request)
        {
            var advertisement = await _advertisementService.AddAdvertisement(request);
            return StatusCode(StatusCodes.Status201Created, advertisement);
        }


        [HttpPatch("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateAdvertisement([FromRoute] Guid id, [FromBody] UpdateAdvertisementRequestDto request)
        {
            var advertisement = await _advertisementService.UpdateAdvertisement(id, request);
            return Ok(advertisement);
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdvertisement([FromRoute] Guid id)
        {
            await _advertisementService.DeleteAdvertisement(id);
            return NoContent();
        }
    }
}
