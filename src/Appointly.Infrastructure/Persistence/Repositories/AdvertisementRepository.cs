using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Advertisments.Contracts;
using Appointly.Domain.Common.Enum;
using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class AdvertisementRepository : IAdvertismentRepository
    {
        private readonly AppointlyDbContext _dbContext;
        public AdvertisementRepository(AppointlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Advertisement> AddAdvertismentAsync(Advertisement advertisment)
        {
            _dbContext.Advertisements.Add(advertisment);
            await  _dbContext.SaveChangesAsync();
            return advertisment;
        }

        public async Task<Advertisement?> GetAdvertismentByIdAsync(Guid id)
        {
            return await _dbContext.Advertisements.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Advertisement>> GetAllAdvertismentsAsync(AdvertisementQuery query)
        {

            IQueryable<Advertisement> q = _dbContext.Advertisements
                                .AsNoTracking()
                                .Include(a => a.Brand)
                                .Include(a => a.Model)
                                .Include(a => a.City)
                                    .ThenInclude(c => c.District)
                                .Include(a => a.City)
                                    .ThenInclude(c => c.Province);

            if (!string.IsNullOrEmpty(query.Search))
            {
                q = q.Where(x =>
                    // PostgreSQL ILIKE for case-insensitive search
                    EF.Functions.ILike(x.Title, $"%{query.Search}%") ||
                    EF.Functions.ILike(x.Description, $"%{query.Search}%") ||
                    EF.Functions.ILike(x.ContactName, $"%{query.Search}%") ||
                    EF.Functions.ILike(x.ContactEmail, $"%{query.Search}%")
                );
            }

            if (query.MinYear.HasValue)
            {
                q = q.Where(x => x.Year >= query.MinYear.Value);
            }

            if (query.MaxYear.HasValue)
            {
                q = q.Where(x => x.Year <= query.MaxYear.Value);
            }

            if (query.MinPrice.HasValue)
            {
                q = q.Where(x => x.Price >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                q = q.Where(x => x.Price <= query.MaxPrice.Value);
            }

            if (query.MinEngineCapacity.HasValue)
            {
                q = q.Where(x => x.EngineCapacity.HasValue && x.EngineCapacity.Value >= query.MinEngineCapacity.Value);
            }

            if (query.MaxEngineCapacity.HasValue)
            {
                q = q.Where(x => x.EngineCapacity.HasValue && x.EngineCapacity.Value <= query.MaxEngineCapacity.Value);
            }

            if (query.FuelType.HasValue)
            {
                q = q.Where(x => x.FuelType == query.FuelType.Value);
            }

            if (query.TransmissionType.HasValue)
            {
                q = q.Where(x => x.TransmissionType == query.TransmissionType.Value);
            }

            if (query.VehicleCondition.HasValue)
            {
                q = q.Where(x => x.VehicleCondition == query.VehicleCondition);

            }

            if (query.Status.HasValue)
            {
                q = q.Where(x => x.Status == query.Status.Value);
            }

            if (query.BrandId.HasValue)
            {
                q = q.Where(x => x.BrandId == query.BrandId.Value);
            }

            if (query.ModelId.HasValue)
            {
                q = q.Where(x => x.ModelId == query.ModelId.Value);
            }

            if (query.ProvinceId.HasValue)
            {
                q = q.Where(x => x.City != null && x.City.ProvinceId == query.ProvinceId.Value);
            }

            if (query.DistrictId.HasValue)
            {
                q = q.Where(x => x.City != null && x.City.DistrictId == query.DistrictId.Value);
            }

            if (query.CityId.HasValue)
            {
                q = q.Where(x => x.CityId == query.CityId.Value);
            }

            if (query.IsBiddable.HasValue)
            {
                q = q.Where(x => x.IsBiddable == query.IsBiddable.Value);
            }

            // Sorting
            var sortBy = query.SortBy ?? SortBy.CreatedDate;
            var sortOrder = query.SortOrder ?? SortOrder.Desc;

            q = (sortBy, sortOrder) switch
            {
                (SortBy.Price, SortOrder.Asc) => q.OrderBy(x => x.Price),
                (SortBy.Price, SortOrder.Desc) => q.OrderByDescending(x => x.Price),
                (SortBy.Year, SortOrder.Asc) => q.OrderBy(x => x.Year),
                (SortBy.Year, SortOrder.Desc) => q.OrderByDescending(x => x.Year),
                (SortBy.EngineCapacity, SortOrder.Asc) => q.OrderBy(x => x.EngineCapacity),
                (SortBy.EngineCapacity, SortOrder.Desc) => q.OrderByDescending(x => x.EngineCapacity),
                (SortBy.Title, SortOrder.Asc) => q.OrderBy(x => x.Title),
                (SortBy.Title, SortOrder.Desc) => q.OrderByDescending(x => x.Title),
                (SortBy.CreatedDate, SortOrder.Asc) => q.OrderBy(x => x.CreatedAt),
                (SortBy.CreatedDate, SortOrder.Desc) => q.OrderByDescending(x => x.CreatedAt),
                _ => q.OrderByDescending(x => x.CreatedAt) // Default sorting
            };


            // Pagination
            var page = query.Page ?? 1;
            var pageSize = query.PageSize ?? 10;

            q = q.Skip((page - 1) * pageSize).Take(pageSize);

            return await q.ToListAsync();
        }


        public async Task<bool> DeleteAdvertisementAsync(Guid id)
        {
            var exisitingAd = await _dbContext.Advertisements.FirstOrDefaultAsync(a => a.Id == id);

            if (exisitingAd == null)
            {
                return false;
            }

            _dbContext.Advertisements.Remove(exisitingAd);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task SaveAdvertisementAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
