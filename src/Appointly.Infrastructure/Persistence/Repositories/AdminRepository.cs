using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Admin.Contracts;
using Appointly.Domain.Common.Constants;
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

            var tasks = identityUsers.Select(async identityUser =>
            {
                var domainUser = domainUsers.FirstOrDefault(u => u.IdentityUserId == identityUser.Id);

                var roles = await _userManager.GetRolesAsync(identityUser);

                return new UserResponse
                {
                    Id = domainUser != null ? domainUser.Id : Guid.Empty,
                    FirstName = domainUser != null ? domainUser.FirstName : null,
                    LastName = domainUser != null ? domainUser.LastName : null,
                    Email = identityUser.Email,
                    PhoneNumber = domainUser != null ? domainUser.PhoneNumber : null,
                    Roles = roles
                };
            }).ToList();

            var result = await Task.WhenAll(tasks);
            return result.ToList();

        }

        public async Task<bool> PromoteToAdminAsync(Guid userId)
        {
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

            if(await _userManager.IsInRoleAsync(identityUser, Roles.User))
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(identityUser, Roles.User);
                if (!removeResult.Succeeded)
                {
                    return false;
                }

                var addResult = await _userManager.AddToRoleAsync(identityUser, Roles.Admin);
                return addResult.Succeeded;
            }
            return false;
        }

        public async Task<bool> PromoteToSellerAsync(Guid userId)
        {
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
                    return false;
            }

            var addResult = await _userManager.AddToRoleAsync(identityUser, Roles.Seller);
            return addResult.Succeeded;
        }
    }
}
