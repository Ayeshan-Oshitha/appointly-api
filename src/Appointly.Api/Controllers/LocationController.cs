using Appointly.Api.Common.DTOs.Location;
using Appointly.Application.Services.Location;
using Appointly.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var provinces = await _locationService.GetProvinces();
            var provinceResponseDtos = _mapper.Map<List<ProvinceResponseDto>>(provinces);
            return Ok(provinceResponseDtos);
        }

        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts([FromQuery] Guid? provinceId)
        {
            var districts = await _locationService.GetDistricts(provinceId);
            var districtResponseDtos = _mapper.Map<List<DistrictResponseDto>>(districts);
            return Ok(districtResponseDtos);
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities([FromQuery] Guid? districtId)
        {
           var cities = await _locationService.GetCities(districtId);
            var cityResponseDtos = _mapper.Map<List<CityResponseDto>>(cities);
            return Ok(cityResponseDtos);
        }

        [HttpGet("citiesByProvince")]
        public async Task<IActionResult> GetCitiesByProvince([FromQuery] Guid? provinceId)
        {
            var cities = await _locationService.GetCitiesByProvince(provinceId);
            return Ok(_mapper.Map<List<CityResponseDto>>(cities));
        }

        [HttpGet("city/{cityId}")]
        public async Task<IActionResult> GetCity([FromRoute] Guid cityId)
        {
            var city = await _locationService.GetCity(cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CityResponseDto>(city));
        }

        [HttpPost("city")]
        public async Task<IActionResult> AddCity([FromBody] AddCityRequestDto request)
        {
            var city = await _locationService.AddCity(request.Name, request.ProvinceId, request.DistrictId);
            return CreatedAtAction(nameof(GetCity), new { cityId = city.Id }, _mapper.Map<CityResponseDto>(city));
        }

        [HttpPut("city/{cityId}")]
        public async Task<IActionResult> UpdateCity([FromRoute] Guid cityId, [FromBody] UpdateCityRequestDto request)
        {
            var updatedCity = await _locationService.UpdateCity(cityId, request.Name, request.ProvinceId, request.DistrictId);
            return Ok(_mapper.Map<CityResponseDto>(updatedCity));
        }

        [HttpDelete("city/{cityId}")]
        public async Task<IActionResult> DeleteCity([FromRoute] Guid cityId)
        {
            await _locationService.DeleteCity(cityId);
            return Ok("City Deleted Successfully");
        }
    }
}
