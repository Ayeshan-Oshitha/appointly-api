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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MotorHubDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
