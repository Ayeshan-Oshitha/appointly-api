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
            var city = await _locationRepository.GetCityById(cityId);
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
            return _mapper.Map<CityResponseDto>(addedCity);
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
                existingCity.Name = request.Name;
                existingCity.Slug = request.Name.Trim().ToLower().Replace(" ", "-");
            }
            if (request.ProvinceId.HasValue || request.DistrictId.HasValue)
            {
                existingCity.ProvinceId = newProvinceId;
                existingCity.DistrictId = newDistrictId;
            }

            await _locationRepository.SaveChangesAsync();
            return _mapper.Map<CityResponseDto>(existingCity);
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
