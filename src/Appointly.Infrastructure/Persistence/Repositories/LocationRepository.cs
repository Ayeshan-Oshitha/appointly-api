using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppointlyDbContext _dbContext;
        public LocationRepository(AppointlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Province>> GetProvincesAsync()
        {
            return await _dbContext.Provinces.AsNoTracking().OrderBy(p => p.Slug).ToListAsync();
        }

        public async Task<List<District>> GetDistrictsByProvinceIdAsync(Guid? provinceId)
        {
            var districts =  _dbContext.Districts.AsNoTracking();

            if (provinceId.HasValue)
            {
                districts = districts.Where(d => d.ProvinceId == provinceId.Value);
            }

            return await districts.OrderBy(d => d.Slug).ToListAsync();
        }

        public async Task<City> GetCityById(Guid cityId)
        {
            var city = await _dbContext.Cities.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cityId);
            return city!;
        }

        public async Task<List<City>> GetCitiesByDistrictIdAsync(Guid? districtId)
        {
            var cities =  _dbContext.Cities.AsNoTracking();

            if (districtId.HasValue)
            {
                cities = cities.Where(c => c.DistrictId == districtId.Value);
            }

            return await cities.OrderBy(c => c.Slug).ToListAsync();
        }

        public async Task<List<City>> GetCitiesByProvienceIdAsync(Guid? provienceId)
        {
            var cities = _dbContext.Cities.AsNoTracking();

            if (provienceId.HasValue)
            {
                cities = cities.Where(c => c.DistrictId == provienceId.Value);
            }

            return await cities.OrderBy(c => c.Slug).ToListAsync();
        }

        public async Task<City> AddCityAsync(City city)
        {
            _dbContext.Cities.Add(city);
            await _dbContext.SaveChangesAsync();
            return city;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
