using Appointly.Application.DTOs.Advertisements;
using Appointly.Application.Services.Advertisements;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("advertisment")]
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
            var advertisments = await _advertisementService.GetAllAdvertisments(query);
            return Ok(advertisments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvertisement([FromBody] CreateAdvertisementRequestDto request)
        {
            var advertisement = await _advertisementService.AddAdvertisment(request);
            return Ok(advertisement);
        }


        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateAdvertisement([FromRoute] Guid id, [FromBody] UpdateAdvertisementRequestDto request)
        {
            var advertisement = await _advertisementService.UpdateAdvertisement(id, request);
            return Ok(advertisement);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAdvertisement([FromRoute] Guid id)
        {
            await _advertisementService.DeleteAdvertisement(id);
            return Ok("Advertisement deleted successfully");
        }
    }
}
