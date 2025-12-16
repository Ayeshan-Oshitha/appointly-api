using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence
{
    public class AppointlyDbContext : DbContext
    {
        public AppointlyDbContext(DbContextOptions<AppointlyDbContext> options)
            :base(options)
        {   
        }

        public DbSet<User> Users { get; set; }
        }
}
