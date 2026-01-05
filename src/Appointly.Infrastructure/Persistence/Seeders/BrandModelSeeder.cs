using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Seeders
{
    public static class BrandModelSeeder
    {
        public static async Task SeedAsync(AppointlyDbContext dbContext)
        {
            // 1. Seed Brands

            if (!await dbContext.Brands.AnyAsync())
            {
                var brands = new List<Brand>
                {
                    new() { Id = Guid.NewGuid(), Name = "Toyota", Slug = "toyota" },
                    new() { Id = Guid.NewGuid(), Name = "Honda", Slug = "honda" },
                    new() { Id = Guid.NewGuid(), Name = "Nissan", Slug = "nissan" },
                    new() { Id = Guid.NewGuid(), Name = "BMW", Slug = "bmw" },
                    new() { Id = Guid.NewGuid(), Name = "Mercedes-Benz", Slug = "mercedes-benz" }
                };

                dbContext.Brands.AddRange(brands);
                await dbContext.SaveChangesAsync();
            }

            // 2. Seed Models
            if(!await dbContext.Models.AnyAsync())
            {
                var brands = await dbContext.Brands.ToDictionaryAsync(b => b.Slug);

                var models = new List<Model>
                {
                    // Toyota
                    new() { Id = Guid.NewGuid(), Name = "Corolla", Slug = "corolla", BrandId = brands["toyota"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Prius", Slug = "prius", BrandId = brands["toyota"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Land Cruiser", Slug = "land-cruiser", BrandId = brands["toyota"].Id },

                    // Honda
                    new() { Id = Guid.NewGuid(), Name = "Civic", Slug = "civic", BrandId = brands["honda"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Accord", Slug = "accord", BrandId = brands["honda"].Id },
                    new() { Id = Guid.NewGuid(), Name = "CR-V", Slug = "cr-v", BrandId = brands["honda"].Id },

                    // Nissan
                    new() { Id = Guid.NewGuid(), Name = "Sunny", Slug = "sunny", BrandId = brands["nissan"].Id },
                    new() { Id = Guid.NewGuid(), Name = "X-Trail", Slug = "x-trail", BrandId = brands["nissan"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Navara", Slug = "navara", BrandId = brands["nissan"].Id },

                    // BMW
                    new() { Id = Guid.NewGuid(), Name = "320i", Slug = "320i", BrandId = brands["bmw"].Id },
                    new() { Id = Guid.NewGuid(), Name = "X5", Slug = "x5", BrandId = brands["bmw"].Id },
                    new() { Id = Guid.NewGuid(), Name = "M3", Slug = "m3", BrandId = brands["bmw"].Id },

                    // Mercedes-Benz
                    new() { Id = Guid.NewGuid(), Name = "C-Class", Slug = "c-class", BrandId = brands["mercedes-benz"].Id },
                    new() { Id = Guid.NewGuid(), Name = "E-Class", Slug = "e-class", BrandId = brands["mercedes-benz"].Id },
                    new() { Id = Guid.NewGuid(), Name = "G-Wagon", Slug = "g-wagon", BrandId = brands["mercedes-benz"].Id }
                };

                dbContext.Models.AddRange(models);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
