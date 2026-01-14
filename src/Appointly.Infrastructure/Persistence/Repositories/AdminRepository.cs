using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Admin.Contracts;
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
    }
}
