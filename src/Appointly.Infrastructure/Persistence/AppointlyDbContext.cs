using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence
{
    public class AppointlyDbContext : DbContext
    {
        public AppointlyDbContext(DbContextOptions<AppointlyDbContext> options)
            : base(options)
        {   
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointlyDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
