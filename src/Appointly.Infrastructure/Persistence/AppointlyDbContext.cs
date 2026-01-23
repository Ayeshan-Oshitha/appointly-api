using Appointly.Domain.Entities;
using Appointly.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence
{
    public class AppointlyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppointlyDbContext(DbContextOptions<AppointlyDbContext> options)
            : base(options)
        {   
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<RoleChangeRequest> RoleChangeRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointlyDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
