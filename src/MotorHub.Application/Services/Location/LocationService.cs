using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Location;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace MotorHub.Application.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        // Get all provinces
        public async Task<List<ProvinceResponseDto>> GetProvinces()
        {
            var provinces = await _locationRepository.GetProvincesAsync();
            return _mapper.Map<List<ProvinceResponseDto>>(provinces);
        }

        // Get all districts or districts by province id
        public async Task<List<DistrictResponseDto>> GetDistricts(Guid? provinceId)
        {
            var districts = await _locationRepository.GetDistrictsByProvinceIdAsync(provinceId);
            return _mapper.Map<List<DistrictResponseDto>>(districts);
        }

        // Get all cities or cities by district id
        public async Task<List<CityResponseDto>> GetCities(Guid? districtId)
        {
            var cities = await _locationRepository.GetCitiesByDistrictIdAsync(districtId);
            return _mapper.Map<List<CityResponseDto>>(cities);
        }

        // Get all cities or cities by province id
        public async Task<List<CityResponseDto>> GetCitiesByProvince(Guid? provinceId)
        {
            var cities = await _locationRepository.GetCitiesByProvienceIdAsync(provinceId);
            return _mapper.Map<List<CityResponseDto>>(cities);
        }

        // Get city by cityId
        public async Task<CityResponseDto?> GetCity(Guid cityId)
        {
            var city = await _locationRepository.GetCityDetailAsync(cityId);
            return city == null ? null : _mapper.Map<CityResponseDto>(city);
        }

        public async Task<CityResponseDto> AddCity(AddCityRequestDto request)
        {
            await ValidateProvinceDistrictRelationship(request.DistrictId, request.ProvinceId);

            var city = new City
            {
                Name = request.Name,
                Slug = request.Name.Trim().ToLower().Replace(" ", "-"),
                ProvinceId = request.ProvinceId,
                DistrictId = request.DistrictId
            };

            if (await _locationRepository.CitySlugExistsAsync(city.Slug))
            {
                throw new ConflictException("City with the same name already exists");
            }

            var addedCity = await _locationRepository.AddCityAsync(city);

            // CityResponseDto carries the province and district *names*, which live on navigation
            // properties a freshly-inserted entity has never loaded. Re-read through GetCityById,
            // which Includes both, instead of mapping the value the write returned.
            return await MapCityWithNavigationsAsync(addedCity.Id);
        }

        public async Task<CityResponseDto> UpdateCity(Guid cityId, UpdateCityRequestDto request)
        {
            var existingCity = await _locationRepository.GetCityById(cityId);

            if (existingCity == null)
            {
                throw new NotFoundException("City not found");
            }

            var newProvinceId = request.ProvinceId ?? existingCity.ProvinceId;
            var newDistrictId = request.DistrictId ?? existingCity.DistrictId;

            await ValidateProvinceDistrictRelationship(newDistrictId, newProvinceId);

            if (!string.IsNullOrEmpty(request.Name))
            {
                var slug = request.Name.Trim().ToLower().Replace(" ", "-");

                // The create path checks this; without the same check here a rename onto an
                // existing city's name reaches the unique index and fails as a 500, not a 409.
                if (await _locationRepository.CitySlugExistsAsync(slug, cityId))
                {
                    throw new ConflictException("City with the same name already exists");
                }

                existingCity.Name = request.Name;
                existingCity.Slug = slug;
            }
            if (request.ProvinceId.HasValue || request.DistrictId.HasValue)
            {
                existingCity.ProvinceId = newProvinceId;
                existingCity.DistrictId = newDistrictId;
            }

            await _locationRepository.SaveChangesAsync();

            // Changing ProvinceId/DistrictId does not move the Province/District navigations with
            // them - the replacement was read with AsNoTracking, so EF has no tracked principal to
            // fix up. Mapping existingCity here would pair the new IDs with the old names.
            return await MapCityWithNavigationsAsync(existingCity.Id);
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
                throw new NotFoundException("Failed to delete city");
            }
        }


        // Reads the city back through the loader that Includes Province and District, so the
        // response always has the names CityResponseDto expects.
        private async Task<CityResponseDto> MapCityWithNavigationsAsync(Guid cityId)
        {
            var city = await _locationRepository.GetCityDetailAsync(cityId);

            if (city == null)
            {
                throw new NotFoundException("City not found");
            }

            return _mapper.Map<CityResponseDto>(city);
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
