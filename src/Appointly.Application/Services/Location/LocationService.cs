using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Location
{
    internal class LocationService : ILocationService
    {
        public Task<City[]> GetCities(Guid districtId)
        {
            throw new NotImplementedException();
        }

        public Task<City[]> GetCitiesByProvince(Guid provinceId)
        {
            throw new NotImplementedException();
        }

        public Task<District[]> GetDistricts(Guid provinceId)
        {
            throw new NotImplementedException();
        }

        public Task<Province[]> GetProvinces()
        {
            throw new NotImplementedException();
        }
    }
}
