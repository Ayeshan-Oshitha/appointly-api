using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Advertisements;
using MotorHub.Domain.Common.Enum;
using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MotorHub.Infrastructure.Persistence.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly MotorHubDbContext _dbContext;
        public AdvertisementRepository(MotorHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Advertisement> AddAdvertisementAsync(Advertisement advertisement)
        {
            _dbContext.Advertisements.Add(advertisement);
            await  _dbContext.SaveChangesAsync();
            return advertisement;
        }

        public async Task<Advertisement?> GetAdvertisementByIdAsync(Guid id)
        {
            return await _dbContext.Advertisements.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Advertisement>> GetAllAdvertisementsAsync(AdvertisementQueryDto query, Guid? sellerScopeId, bool isAdmin)
        {

            IQueryable<Advertisement> q = _dbContext.Advertisements
                                .AsNoTracking()
                                .Include(a => a.Brand)
                                .Include(a => a.Model)
                                .Include(a => a.City)
                                    .ThenInclude(c => c.District)
                                .Include(a => a.City)
                                    .ThenInclude(c => c.Province)
                                .Include(a => a.Seller)
                                .Include(a => a.ReviewByAdmin);


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

            // Soft-deleted rows are already excluded by the global query filter.

            // Visibility gate. Anonymous callers only ever see Active ads; a signed-in seller
            // additionally sees their own in any status (so they can read RejectedReason on a
            // rejection); admins see everything. The caller's own Status filter below then
            // narrows *within* this set rather than escaping it.
            if (!isAdmin)
            {
                q = sellerScopeId.HasValue
                    ? q.Where(x => x.Status == AdStatus.Active || x.SellerId == sellerScopeId.Value)
                    : q.Where(x => x.Status == AdStatus.Active);
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
            // page is clamped as well as pageSize: an unclamped ?page=-5 becomes a negative
            // Skip, which Postgres rejects as a negative OFFSET.
            var page = Math.Max(query.Page ?? 1, 1);
            var pageSize = Math.Clamp(query.PageSize ?? 10, 1, 100);

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

            // Soft delete: the ad stays for audit/history, and the global query filter on
            // Advertisement keeps it out of every read.
            exisitingAd.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task SaveAdvertisementAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
