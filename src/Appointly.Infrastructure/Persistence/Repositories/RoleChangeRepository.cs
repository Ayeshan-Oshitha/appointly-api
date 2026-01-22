using Appointly.Application.Common.Interfaces.Persistence;
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
    }
}
