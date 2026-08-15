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
            var identityUsers = await _userManager.Users.ToListAsync();

            var identityUserIds = identityUsers.Select(u => u.Id).ToList();

            var domainUsers = await _dbContext.DomainUsers
                .Where(u => identityUserIds.Contains(u.IdentityUserId)).ToListAsync();

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

        public async Task<bool> PromoteToAdminAsync(Guid userId, Guid changeRoleRequestId, Guid currrentUserId)
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
                        throw new Exception("Failed to assign new role.");
                    }
                }

                // Update Role Change Request Status
                request.Status = RoleRequestTypes.Approved;
                request.ReviewedByAdminId = currrentUserId;
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

        public async Task<bool> PromoteToSellerAsync(Guid userId, Guid changeRoleRequestId, Guid currrentUserId)
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
                        throw new Exception("Failed to assign new role.");
                    }
                }

                // Update Role Change Request Status
                request.Status = RoleRequestTypes.Approved;
                request.ReviewedByAdminId = currrentUserId;
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

        public async Task<bool> RejectPromoteRequestAsync(Guid changeRoleRequestId, Guid currrentUserId, string? rejectReason)
        {
            // Load Request 
            var request = await _dbContext.RoleChangeRequests.FirstOrDefaultAsync(x => x.Id == changeRoleRequestId);

            if (request == null)
            {
                throw new NotFoundException("Role change request not found.");
            }

            request.Status = RoleRequestTypes.Rejected;
            request.RejectionReason = rejectReason ?? null;
            request.ReviewedByAdminId = currrentUserId;
            request.ReviewedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<RoleChangeRequest?> GetExistingPendingRequestsByUserIdAsync(Guid userId)
        {
            return await _dbContext.RoleChangeRequests
                .FirstOrDefaultAsync(r => r.UserId == userId && r.Status == RoleRequestTypes.Pending);
        }

        public async Task<Advertisement> ApproveAdvertisementAsync(Guid AdvertisementId, Guid currentUserId)
        {
            var existingAd = await _dbContext.Advertisements.FirstOrDefaultAsync(x => x.Id == AdvertisementId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            existingAd.Status = AdStatus.Active;
            existingAd.ReviewByAdminId = currentUserId;
            existingAd.ReviewedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return existingAd;
        }

        public async Task<Advertisement> RejectAdvertisementAsync(Guid AdvertisementId, Guid currentUserId, string? reason)
        {
            var existingAd = await _dbContext.Advertisements.FirstOrDefaultAsync(x => x.Id == AdvertisementId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            existingAd.Status = AdStatus.Rejected;
            existingAd.RejectedReason = reason ?? null;
            existingAd.ReviewByAdminId = currentUserId;
            existingAd.ReviewedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return existingAd;
        }

        public async Task<Advertisement> UndoAdvertisementReviewAsync(Guid AdvertisementId)
        {
            var existingAd = await _dbContext.Advertisements.FirstOrDefaultAsync(x => x.Id == AdvertisementId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found.");
            }

            existingAd.Status = AdStatus.Pending;
            existingAd.RejectedReason = null;
            existingAd.ReviewByAdminId = null;
            existingAd.ReviewedAt = null;

            await _dbContext.SaveChangesAsync();
            return existingAd;
        }
    }
}
