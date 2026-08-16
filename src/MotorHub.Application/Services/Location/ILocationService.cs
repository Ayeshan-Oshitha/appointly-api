using MotorHub.Application.DTOs.Location;

namespace MotorHub.Application.Services.Location
{
    public interface ILocationService
    {
        Task<List<ProvinceResponseDto>> GetProvinces();
        Task<List<DistrictResponseDto>> GetDistricts(Guid? provinceId);
        Task<List<CityResponseDto>> GetCities(Guid? districtId);
        Task<List<CityResponseDto>> GetCitiesByProvince(Guid? provinceId);
        Task<CityResponseDto?> GetCity(Guid cityId);
        Task<CityResponseDto> AddCity(AddCityRequestDto request);
        Task<CityResponseDto> UpdateCity(Guid cityId, UpdateCityRequestDto request);
        Task DeleteCity(Guid cityId);

    }
}
