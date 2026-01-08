using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Location
{
    public interface ILocationService
    {
        Task<List<Province>> GetProvinces();
        Task<List<District>> GetDistricts(Guid? provinceId);
        Task<List<City>> GetCities(Guid? districtId);
        Task<List<City>> GetCitiesByProvince(Guid? provinceId);
        Task<City?> GetCity(Guid cityId);
        Task<City> AddCity(string name, Guid provienceId, Guid districtId);
        Task<City> UpdateCity(Guid cityId, string? name, Guid? provienceId, Guid? districtId);
        Task DeleteCity(Guid cityId);

    }
}
