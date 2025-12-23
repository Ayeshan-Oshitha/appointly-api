using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Location
{
    public interface ILocationService
    {
        Task<Province[]> GetProvinces();
        Task<District[]> GetDistricts(Guid provinceId);
        Task<City[]> GetCities(Guid districtId);
        Task<City[]> GetCitiesByProvince(Guid provinceId);
    }
}
