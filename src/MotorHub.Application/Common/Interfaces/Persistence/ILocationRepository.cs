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

        // Untracked read with Province and District included. Used for responses - notably after a
        // write, where the tracked instance still holds the navigations it was loaded with and
        // would report the old province/district names alongside the new IDs.
        public Task<City?> GetCityDetailAsync(Guid cityId);
        public Task<City> AddCityAsync(City city);
        public Task<bool> DeleteAsync(Guid cityId);
        // See IBrandRepository.BrandSlugExistsAsync for why the exclusion exists.
        Task<bool> CitySlugExistsAsync(string slug, Guid? excludeCityId = null);
        public Task SaveChangesAsync();

        

    }
}
