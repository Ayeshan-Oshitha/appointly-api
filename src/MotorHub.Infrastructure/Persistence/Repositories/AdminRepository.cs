using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Admin;
using MotorHub.Domain.Common.Constants;
using MotorHub.Domain.Common.Enum;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MotorHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MotorHub.Infrastructure.Persistence.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MotorHubDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminRepository(MotorHubDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var identityUsers = await _userManager.Users.AsNoTracking().ToListAsync();

            var identityUserIds = identityUsers.Select(u => u.Id).ToList();

            var domainUsers = await _dbContext.DomainUsers
                .Where(u => identityUserIds.Contains(u.IdentityUserId)).AsNoTracking().ToListAsync();

            var result = new List<UserResponseDto>();

            foreach (var identityUser in identityUsers)
            {
                var domainUser = domainUsers.FirstOrDefault(u => u.IdentityUserId == identityUser.Id);

                var roles = await _userManager.GetRolesAsync(identityUser); // awaited one-by-one

                result.Add(new UserResponseDto
                {
                    Id = domainUser != null ? domainUser.Id : Guid.Empty,
                    FirstName = domainUser?.FirstName,
                    LastName = domainUser?.LastName,
                    Email = identityUser.Email,
                    PhoneNumber = domainUser?.PhoneNumber,
                    Roles = roles
                });
            }

            return result;
        }

        public async Task<bool> PromoteToAdminAsync(Guid userId, Guid changeRoleRequestId, Guid currentUserId)
        {
            // Load Request 
            var request = await _dbContext.RoleChangeRequests.FirstOrDefaultAsync(x => x.Id == changeRoleRequestId);

            if (request == null)
            {
                throw new NotFoundException("Role change request not found.");
            }

            // Load User
            var user = await _dbContext.DomainUsers.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId.ToString());

            if (identityUser == null)
            {
                throw new NotFoundException("Identity User not found.");
            }

            // AddToRoleAsync commits on its own, so the role grant and the request status update
            // need one transaction between them - otherwise a mid-sequence failure leaves the
            // user promoted with the request still Pending, or the reverse.
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Roles are additive: User is the baseline every account keeps, Seller and Admin are
                // tiers layered on top. Removing the existing roles here would strip User and lock the
                // promoted account out of every User-gated endpoint.
                if (!await _userManager.IsInRoleAsync(identityUser, Roles.Admin))
                {
                    var addResult = await _userManager.AddToRoleAsync(identityUser, Roles.Admin);
                    if (!addResult.Succeeded)
                    {
                        throw new BadRequestException("Failed to assign new role.");
                    }
                }

                // Update Role Change Request Status
                request.Status = RoleRequestTypes.Approved;
                request.ReviewedByAdminId = currentUserId;
                request.ReviewedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> PromoteToSellerAsync(Guid userId, Guid changeRoleRequestId, Guid currentUserId)
        {
            // Load Request 
            var request = await _dbContext.RoleChangeRequests.FirstOrDefaultAsync(x => x.Id == changeRoleRequestId);

            if (request == null)
            {
                throw new NotFoundException("Role change request not found.");
            }

            // Load User
            var user = await _dbContext.DomainUsers.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId.ToString());

            if (identityUser == null)
            {
                throw new NotFoundException("Identity User not found.");
            }

            // Transactional for the same reason as PromoteToAdminAsync above.
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Additive, for the same reason as PromoteToAdminAsync above.
                if (!await _userManager.IsInRoleAsync(identityUser, Roles.Seller))
                {
                    var addResult = await _userManager.AddToRoleAsync(identityUser, Roles.Seller);
                    if (!addResult.Succeeded)
                    {
                        throw new BadRequestException("Failed to assign new role.");
                    }
                }

                // Update Role Change Request Status
                request.Status = RoleRequestTypes.Approved;
                request.ReviewedByAdminId = currentUserId;
                request.ReviewedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> RejectPromoteRequestAsync(Guid changeRoleRequestId, Guid currentUserId, string? rejectReason)
        {
            // Load Request 
            var request = await _dbContext.RoleChangeRequests.FirstOrDefaultAsync(x => x.Id == changeRoleRequestId);

            if (request == null)
            {
                throw new NotFoundException("Role change request not found.");
            }

            request.Status = RoleRequestTypes.Rejected;
            request.RejectionReason = rejectReason ?? null;
            request.ReviewedByAdminId = currentUserId;
            request.ReviewedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        // The three review transitions below carry their precondition in the UPDATE's WHERE clause
        // rather than checking it first and writing after. AdminService does check the status, but
        // that check and the write are separate round trips: two admins reviewing the same ad both
        // passed the check and both wrote, so one decision was silently overwritten. Gating on the
        // expected status makes the transition a single atomic compare-and-swap - the loser now
        // affects zero rows and gets a 409 instead of quietly losing.
        public async Task<Advertisement> ApproveAdvertisementAsync(Guid AdvertisementId, Guid currentUserId)
        {
            var reviewedAt = DateTime.UtcNow;

            var rowsAffected = await _dbContext.Advertisements
                .Where(x => x.Id == AdvertisementId && x.Status == AdStatus.Pending)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, AdStatus.Active)
                    .SetProperty(x => x.ReviewByAdminId, (Guid?)currentUserId)
                    .SetProperty(x => x.ReviewedAt, (DateTime?)reviewedAt));

            if (rowsAffected == 0)
            {
                throw await ReviewFailureAsync(AdvertisementId);
            }

            return await ReloadAdvertisementAsync(AdvertisementId);
        }

        public async Task<Advertisement> RejectAdvertisementAsync(Guid AdvertisementId, Guid currentUserId, string? reason)
        {
            var reviewedAt = DateTime.UtcNow;

            var rowsAffected = await _dbContext.Advertisements
                .Where(x => x.Id == AdvertisementId && x.Status == AdStatus.Pending)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, AdStatus.Rejected)
                    .SetProperty(x => x.RejectedReason, reason)
                    .SetProperty(x => x.ReviewByAdminId, (Guid?)currentUserId)
                    .SetProperty(x => x.ReviewedAt, (DateTime?)reviewedAt));

            if (rowsAffected == 0)
            {
                throw await ReviewFailureAsync(AdvertisementId);
            }

            return await ReloadAdvertisementAsync(AdvertisementId);
        }

        public async Task<Advertisement> UndoAdvertisementReviewAsync(Guid AdvertisementId)
        {
            var rowsAffected = await _dbContext.Advertisements
                .Where(x => x.Id == AdvertisementId && x.Status != AdStatus.Pending)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, AdStatus.Pending)
                    .SetProperty(x => x.RejectedReason, (string?)null)
                    .SetProperty(x => x.ReviewByAdminId, (Guid?)null)
                    .SetProperty(x => x.ReviewedAt, (DateTime?)null));

            if (rowsAffected == 0)
            {
                throw await ReviewFailureAsync(AdvertisementId,
                    "Advertisement review was already undone by another admin.");
            }

            return await ReloadAdvertisementAsync(AdvertisementId);
        }

        // Zero rows affected means either the ad is gone or another admin got there first;
        // only a second lookup can tell those apart, and it only runs on the failure path.
        private async Task<Exception> ReviewFailureAsync(
            Guid advertisementId,
            string conflictMessage = "Advertisement was already reviewed by another admin.")
        {
            var exists = await _dbContext.Advertisements
                .AsNoTracking()
                .AnyAsync(x => x.Id == advertisementId);

            return exists
                ? new ConflictException(conflictMessage)
                : new NotFoundException("Advertisement not found.");
        }

        // ExecuteUpdateAsync writes straight to the database and never touches the change tracker,
        // so the instance AdminService loaded before calling in is now stale. Reload untracked -
        // a tracked query would resolve back to that same stale instance and the response would
        // show the pre-update status.
        private async Task<Advertisement> ReloadAdvertisementAsync(Guid advertisementId)
        {
            var advertisement = await _dbContext.Advertisements
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == advertisementId);

            if (advertisement == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            return advertisement;
        }
    }
}
