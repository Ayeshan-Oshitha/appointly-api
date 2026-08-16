using MotorHub.Domain.Entities;
using MotorHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MotorHub.Infrastructure.Persistence
{
    public class MotorHubDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public MotorHubDbContext(DbContextOptions<MotorHubDbContext> options)
            : base(options)
        {   
        }

        // Deliberately not named Users: IdentityDbContext already exposes
        // DbSet<ApplicationUser> Users, and shadowing it makes _dbContext.Users mean the
        // opposite of what an Identity-literate reader expects.
        public DbSet<User> DomainUsers { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<RoleChangeRequest> RoleChangeRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Base first: IdentityDbContext configures its own entities here, and applying this
            // project's configurations afterwards means they win on anything that overlaps.
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MotorHubDbContext).Assembly);
        }
    }
}
