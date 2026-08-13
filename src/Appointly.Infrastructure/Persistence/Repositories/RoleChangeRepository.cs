using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.DTOs.RoleChange;
using Appointly.Domain.Common.Constants;
using Appointly.Domain.Common.Enum;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class RoleChangeRepository : IRoleChangeRepository
    {
        private readonly AppointlyDbContext _dbContext;
        public RoleChangeRepository(AppointlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRoleChangeRequestAsync(Guid userId, string newRole)
        {
            var hasPendingRequests = await _dbContext.RoleChangeRequests.AnyAsync(
                x => x.UserId == userId && x.Status == RoleRequestTypes.Pending);

            if (hasPendingRequests)
            {
                throw new BadRequestException("There is already a pending role change request for this user.");
            }

            var roleChangeRequest = new RoleChangeRequest
            {
                UserId = userId,
                RequestedRole = newRole,
                Status = RoleRequestTypes.Pending,
                RequestedAt = DateTime.UtcNow
            };

            _dbContext.RoleChangeRequests.Add(roleChangeRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<RoleChangeRequest>> GetAllRequestsAsync(RoleChangeRequestQueryDto query)
        {
            IQueryable<RoleChangeRequest> q = _dbContext.RoleChangeRequests
                                    .AsNoTracking()
                                    .Include(r => r.User)
                                    .Include(r => r.ReviewedByAdmin);

            if (!string.IsNullOrEmpty(query.Search))
            {
                q = q.Where(x =>
                EF.Functions.ILike(x.User.FirstName, $"%{query.Search}%") ||
                EF.Functions.ILike(x.User.LastName, $"%{query.Search}%")
                );
            }

            if (query.RequestType.HasValue)
            {
                q = q.Where(x => x.Status == query.RequestType.Value);
            }

            if (!string.IsNullOrEmpty(query.RoleType))  
            {
                if (!Roles.All.Contains(query.RoleType))
                {
                    throw new BadRequestException("Invalid role type.");
                }
                q = q.Where(x => x.RequestedRole == query.RoleType);
            }

            if (query.UserId.HasValue)
            {
                q = q.Where(x => x.UserId == query.UserId.Value);
            }

            if (query.ReviewByAdminId.HasValue)
            {
                q = q.Where(x => x.ReviewedByAdminId == query.ReviewByAdminId.Value);
            }

            if (query.FromDate.HasValue)
            {
                var fromDateUtc = DateTime.SpecifyKind(
                    query.FromDate.Value, DateTimeKind.Utc);

                q = q.Where(x => x.RequestedAt >= fromDateUtc);
            }

            if (query.ToDate.HasValue)
            {
                var toDateUtc = DateTime.SpecifyKind(
                    query.ToDate.Value.Date.AddDays(1),
                    DateTimeKind.Utc);

                q = q.Where(x => x.RequestedAt < toDateUtc);
            }

            // Sorting
            var sortBy = query.SortBy ?? RoleChangeSortBy.CreatedAt;
            var sortOrder = query.SortOrder ?? RoleChangeSortOrder.Desc;

            q = (sortBy, sortOrder) switch
            {
                (RoleChangeSortBy.CreatedAt, RoleChangeSortOrder.Asc) => q.OrderBy(x => x.RequestedAt),
                (RoleChangeSortBy.CreatedAt, RoleChangeSortOrder.Desc) => q.OrderByDescending(x => x.RequestedAt),
                (RoleChangeSortBy.FirstName, RoleChangeSortOrder.Asc) => q.OrderBy(x => x.User != null ? x.User.FirstName ?? "" : ""),
                (RoleChangeSortBy.FirstName, RoleChangeSortOrder.Desc) => q.OrderByDescending(x => x.User != null ? x.User.FirstName ?? "" : ""),
                _ => q.OrderByDescending(x => x.RequestedAt)
            };

            // Pagination
            var page = query.Page ?? 1;
            var pageSize = Math.Clamp(query.PageSize ?? 10, 1, 100);

            q = q.Skip((page - 1) * pageSize).Take(pageSize);

            return await q.ToListAsync();
        }

        public async Task<RoleChangeRequest?> GetRoleChangeRequestByIdAsync(Guid id)
        {
            return await _dbContext.RoleChangeRequests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteRoleChangeRequestAsync(Guid id)
        {
            var existingRequest = await _dbContext.RoleChangeRequests.FirstOrDefaultAsync(x => x.Id == id);
            
            if (existingRequest == null)
            {
                throw new NotFoundException("Role change request not found.");
            }

            _dbContext.RoleChangeRequests.Remove(existingRequest);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
