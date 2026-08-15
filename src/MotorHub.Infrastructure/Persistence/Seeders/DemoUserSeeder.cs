using MotorHub.Domain.Common.Constants;
using MotorHub.Domain.Entities;
using MotorHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MotorHub.Infrastructure.Persistence.Seeders
{
    /// <summary>
    /// Creates the well-known demo seller used by <see cref="AdvertisementSeeder"/>.
    /// Development-only data.
    ///
    /// A seller needs BOTH an Identity row (AspNetUsers) and a domain row ("Users"),
    /// exactly as UserRepository.AddUserAsync does at runtime. RoleSeeder must have run
    /// first, otherwise the role assignment below fails.
    /// </summary>
    public static class DemoUserSeeder
    {
        public const string SellerEmail = "demo.seller@motorhub.local";

        private const string SellerFirstName = "Demo";
        private const string SellerLastName = "Seller";
        private const string SellerPhone = "0770000000";

        public static async Task<User> SeedAsync(
            MotorHubDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            string password,
            ILogger logger,
            CancellationToken cancellationToken = default)
        {
            var identityUser = await userManager.FindByEmailAsync(SellerEmail);

            if (identityUser is null)
            {
                identityUser = new ApplicationUser
                {
                    UserName = SellerEmail,
                    Email = SellerEmail,
                    PhoneNumber = SellerPhone,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(identityUser, password);

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to create the demo seller '{SellerEmail}': " +
                        string.Join("; ", createResult.Errors.Select(e => e.Description)));
                }

                logger.LogInformation("Demo seller identity created: {Email}", SellerEmail);
            }

            foreach (var role in new[] { Roles.User, Roles.Seller })
            {
                if (await userManager.IsInRoleAsync(identityUser, role))
                {
                    continue;
                }

                var roleResult = await userManager.AddToRoleAsync(identityUser, role);

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to add the demo seller to role '{role}': " +
                        string.Join("; ", roleResult.Errors.Select(e => e.Description)) +
                        " (has RoleSeeder run?)");
                }
            }

            var domainUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id, cancellationToken);

            if (domainUser is null)
            {
                // User.Id is assigned inside the constructor and has a private setter,
                // so it can never be hard-coded - the seller must always be looked up.
                domainUser = new User(identityUser.Id, SellerFirstName, SellerLastName, SellerPhone);

                await dbContext.Users.AddAsync(domainUser, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Demo seller domain user created: {UserId}", domainUser.Id);
            }

            return domainUser;
        }
    }
}
