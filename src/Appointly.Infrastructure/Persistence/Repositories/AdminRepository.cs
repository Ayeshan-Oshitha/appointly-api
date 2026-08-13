using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Admin.Contracts;
using Appointly.Domain.Common.Constants;
using Appointly.Domain.Common.Enum;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using Appointly.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppointlyDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminRepository(AppointlyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            var identityUsers = await _userManager.Users.ToListAsync();

            var identityUserIds = identityUsers.Select(u => u.Id).ToList();

            var domainUsers = await _dbContext.Users
                .Where(u => identityUserIds.Contains(u.IdentityUserId)).ToListAsync();

            var result = new List<UserResponse>();

            foreach (var identityUser in identityUsers)
            {
                var domainUser = domainUsers.FirstOrDefault(u => u.IdentityUserId == identityUser.Id);

                var roles = await _userManager.GetRolesAsync(identityUser); // awaited one-by-one

                result.Add(new UserResponse
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId.ToString());

            if (identityUser == null)
            {
                throw new NotFoundException("Identity User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(identityUser);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);
                if (!removeResult.Succeeded)
                {
                    throw new Exception("Failed to remove existing roles.");
                }
            }

            var addResult = await _userManager.AddToRoleAsync(identityUser, Roles.Admin);
            if (!addResult.Succeeded)
            {
                throw new Exception("Failed to assign new role.");
            }

            // Update Role Change Request Status
            request.Status = RoleRequestTypes.Approved;
            request.ReviewedByAdminId = currrentUserId;
            request.ReviewedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId.ToString());

            if (identityUser == null)
            {
                throw new NotFoundException("Identity User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(identityUser);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);
                if (!removeResult.Succeeded) 
                { 
                    throw new Exception("Failed to remove existing roles.");
                }
            }

            var addResult = await _userManager.AddToRoleAsync(identityUser, Roles.Seller);
            if (!addResult.Succeeded)
            {
                throw new Exception("Failed to assign new role.");
            }

            // Update Role Change Request Status
            request.Status= RoleRequestTypes.Approved;
            request.ReviewedByAdminId= currrentUserId;
            request.ReviewedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
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

        public async Task<Advertisement> ApproveAdvertisementAsync(Guid AdvertismentId, Guid currentUserId)
        {
            var existingAd = await _dbContext.Advertisements.FirstOrDefaultAsync(x => x.Id == AdvertismentId);

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

        public async Task<Advertisement> RejectAdvertisementAsync(Guid AdvertismentId, Guid currentUserId, string? reason)
        {
            var existingAd = await _dbContext.Advertisements.FirstOrDefaultAsync(x => x.Id == AdvertismentId);

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

        public Task<Advertisement> BlockAdvertisementAsync(Guid AdvertismentId, Guid currentUserId, string? reason)
        {
            throw new NotImplementedException();
        }

        public async Task<Advertisement> UndoAdvertismentReviewAsync(Guid AdvertismentId)
        {
            var existingAd = await _dbContext.Advertisements.FirstOrDefaultAsync(x => x.Id == AdvertismentId);

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
