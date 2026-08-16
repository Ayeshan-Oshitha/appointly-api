using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MotorHub.Infrastructure.Persistence.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly MotorHubDbContext _dbContext;
        public LocationRepository(MotorHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Province>> GetProvincesAsync()
        {
            return await _dbContext.Provinces.AsNoTracking().OrderBy(p => p.Slug).ToListAsync();
        }

        public async Task<List<District>> GetDistrictsByProvinceIdAsync(Guid? provinceId)
        {
            IQueryable<District> query =  _dbContext.Districts.AsNoTracking().Include(p => p.Province);

            if (provinceId.HasValue)
            {
                query = query.Where(d => d.ProvinceId == provinceId.Value);
            }

            return await query.OrderBy(d => d.Slug).ToListAsync();
        }

        public async Task<District?> GetDistrictByIdAsync(Guid districtId)
        {
            var district = await _dbContext.Districts.AsNoTracking().FirstOrDefaultAsync(d => d.Id == districtId);
            return district;
        }

        public async Task<City?> GetCityById(Guid cityId)
        {
            var city = await _dbContext.Cities.Include(c => c.Province).Include(c => c.District).FirstOrDefaultAsync(c => c.Id == cityId);
            return city;
        }

        public async Task<List<City>> GetCitiesByDistrictIdAsync(Guid? districtId)
        {
            IQueryable<City> cities =  _dbContext.Cities.AsNoTracking().Include(c => c.Province).Include(c => c.District);

            if (districtId.HasValue)
            {
                cities = cities.Where(c => c.DistrictId == districtId.Value);
            }

            return await cities.OrderBy(c => c.Slug).ToListAsync();
        }

        public async Task<List<City>> GetCitiesByProvienceIdAsync(Guid? provinceId)
        {
            IQueryable<City> cities = _dbContext.Cities.AsNoTracking().Include(c => c.Province).Include(c => c.District);

            if (provinceId.HasValue)
            {
                cities = cities.Where(c => c.ProvinceId == provinceId.Value);
            }

            return await cities.OrderBy(c => c.Slug).ToListAsync();
        }

        public async Task<City> AddCityAsync(City city)
        {

            _dbContext.Cities.Add(city);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            // Slug uniqueness is checked in the service, but that check and this save are not
            // atomic; see the same guard in BrandRepository.AddBrandAsync.
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new ConflictException("City with the same name already exists");
            }

            return city;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid cityId)
        {
            var existingCity = await _dbContext.Cities.FirstOrDefaultAsync(c => c.Id == cityId);

            if (existingCity == null)
            {
                return false;
            }

            _dbContext.Cities.Remove(existingCity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task<bool> CitySlugExistsAsync(string slug)
        {
            return _dbContext.Cities.AnyAsync(c => c.Slug == slug);
        }
    }
}
