using MotorHub.Domain.Entities;

namespace MotorHub.Application.Common.Interfaces.Persistence
{
    public interface ILocationRepository
    {
        public Task<List<Province>> GetProvincesAsync();
        public Task<List<District>> GetDistrictsByProvinceIdAsync(Guid? provinceId);
        public Task<List<City>> GetCitiesByDistrictIdAsync(Guid? districtId);
        public Task<List<City>> GetCitiesByProvienceIdAsync(Guid? provienceId);
        public Task<District?> GetDistrictByIdAsync(Guid districtId);
        public Task<City?> GetCityById(Guid cityId);
        public Task<City> AddCityAsync(City city);
        public Task<bool> DeleteAsync(Guid cityId);
        Task<bool> CitySlugExistsAsync(string slug);
        public Task SaveChangesAsync();

        

    }
}
