using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        // Get all provinces
        public async Task<List<Province>> GetProvinces()
        {
            return await _locationRepository.GetProvincesAsync();
        }

        // Get all districts or districts by province id
        public async Task<List<District>> GetDistricts(Guid? provinceId)
        {
            return await _locationRepository.GetDistrictsByProvinceIdAsync(provinceId);
        }

        // Get all cities or cities by district id
        public async Task<List<City>> GetCities(Guid? districtId)
        {
            return await _locationRepository.GetCitiesByDistrictIdAsync(districtId);
        }

        // Get all cities or cities by province id
        public async Task<List<City>> GetCitiesByProvince(Guid? provinceId)
        {
            return await _locationRepository.GetCitiesByProvienceIdAsync(provinceId);
        }

        // Get city by cityId
        public async Task<City?> GetCity(Guid cityId)
        {
            return await _locationRepository.GetCityById(cityId);
        }

        public async Task<City> AddCity(string name, Guid provienceId, Guid districtId)
        {
            await ValidateProvinceDistrictRelationship(districtId, provienceId);

            var city = new City
            {
                Name = name,
                Slug = name.Trim().ToLower().Replace(" ", "-"),
                ProvinceId = provienceId,
                DistrictId = districtId
            };

            if (await _locationRepository.CitySlugExistsAsync(city.Slug))
            {
                throw new BadRequestException("City with the same name already exists");
            }

            var addedCity = await _locationRepository.AddCityAsync(city);
            return addedCity;
        }

        public async Task<City> UpdateCity(Guid cityId, string? name, Guid? provienceId, Guid? districtId)
        {
            var existingCity = await _locationRepository.GetCityById(cityId);

            if (existingCity == null)
            {
                throw new NotFoundException("City not found");
            } 

            var newProvinceId = provienceId ?? existingCity.ProvinceId;
            var newDistrictId = districtId ?? existingCity.DistrictId;

            await ValidateProvinceDistrictRelationship(newDistrictId, newProvinceId);

            if (!string.IsNullOrEmpty(name))
            {
                existingCity.Name = name;
                existingCity.Slug = name.ToLower().Replace(" ", "-");
            }
            if (provienceId.HasValue || districtId.HasValue)
            {
                existingCity.ProvinceId = newProvinceId;
                existingCity.DistrictId = newDistrictId;
            }

            await _locationRepository.SaveChangesAsync();
            return existingCity;
        }

        public async Task DeleteCity(Guid cityId)
        {
            var existingCity = await _locationRepository.GetCityById(cityId);

            if (existingCity == null)
            {
                throw new NotFoundException("City not found");
            }

            var deleted = await _locationRepository.DeleteAsync(cityId);

            if (!deleted)
            {
                throw new Exception("Failed to delete city");
            }
        }


        private async Task ValidateProvinceDistrictRelationship(Guid districtId, Guid provienceId)
        {
            var district = await _locationRepository.GetDistrictByIdAsync(districtId);
            if (district == null)
            {
                throw new NotFoundException("District not found");
            }
            if (district.ProvinceId != provienceId)
            {
                throw new BadRequestException("District does not belong to the specified province");
            }
        }
    }
}
