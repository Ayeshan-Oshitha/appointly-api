using MotorHub.Domain.Common.Constants;
using MotorHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace MotorHub.Infrastructure.Persistence.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var role in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role,
                    });
                }
            }
        }
    }
}
