using Appointly.Application.DTOs.Location;
using Appointly.Application.Services.Location;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var provinces = await _locationService.GetProvinces();
            return Ok(provinces);
        }

        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts([FromQuery] Guid? provinceId)
        {
            var districts = await _locationService.GetDistricts(provinceId);
            return Ok(districts);
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities([FromQuery] Guid? districtId)
        {
            var cities = await _locationService.GetCities(districtId);
            return Ok(cities);
        }

        [HttpGet("citiesByProvince")]
        public async Task<IActionResult> GetCitiesByProvince([FromQuery] Guid? provinceId)
        {
            var cities = await _locationService.GetCitiesByProvince(provinceId);
            return Ok(cities);
        }

        [HttpGet("city/{cityId}")]
        public async Task<IActionResult> GetCity([FromRoute] Guid cityId)
        {
            var city = await _locationService.GetCity(cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpPost("city")]
        public async Task<IActionResult> AddCity([FromBody] AddCityRequestDto request)
        {
            var city = await _locationService.AddCity(request);
            return CreatedAtAction(nameof(GetCity), new { cityId = city.Id }, city);
        }

        [HttpPut("city/{cityId}")]
        public async Task<IActionResult> UpdateCity([FromRoute] Guid cityId, [FromBody] UpdateCityRequestDto request)
        {
            var updatedCity = await _locationService.UpdateCity(cityId, request);
            return Ok(updatedCity);
        }

        [HttpDelete("city/{cityId}")]
        public async Task<IActionResult> DeleteCity([FromRoute] Guid cityId)
        {
            await _locationService.DeleteCity(cityId);
            return Ok("City Deleted Successfully");
        }
    }
}
