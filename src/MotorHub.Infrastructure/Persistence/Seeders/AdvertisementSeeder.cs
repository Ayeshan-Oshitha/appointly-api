using MotorHub.Domain.Common.Enum;
using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MotorHub.Infrastructure.Persistence.Seeders
{
    /// <summary>
    /// Demo advertisement data. Development-only.
    ///
    /// Parents are resolved by slug rather than by hard-coded GUID: LocationSeeder and
    /// BrandModelSeeder mint fresh IDs with Guid.NewGuid() on every seed run, so any
    /// literal GUID here is only ever valid on the machine it was copied from.
    ///
    /// Requires LocationSeeder, BrandModelSeeder and DemoUserSeeder to have run first.
    /// </summary>
    public static class AdvertisementSeeder
    {
        public static async Task SeedAsync(
            MotorHubDbContext dbContext,
            Guid sellerId,
            ILogger logger,
            CancellationToken cancellationToken = default)
        {
            if (await dbContext.Advertisements.AnyAsync(cancellationToken))
            {
                logger.LogDebug("Advertisements already present; skipping demo advertisement seed.");
                return;
            }

            var cities = await dbContext.Cities.ToDictionaryAsync(c => c.Slug, cancellationToken);
            var brands = await dbContext.Brands.ToDictionaryAsync(b => b.Slug, cancellationToken);
            var models = await dbContext.Models.ToDictionaryAsync(m => m.Slug, cancellationToken);

            Guid CityId(string slug) => cities.TryGetValue(slug, out var city)
                ? city.Id
                : throw new InvalidOperationException(
                    $"Demo advertisement data references unknown city slug '{slug}'. Has LocationSeeder run?");

            Guid BrandId(string slug) => brands.TryGetValue(slug, out var brand)
                ? brand.Id
                : throw new InvalidOperationException(
                    $"Demo advertisement data references unknown brand slug '{slug}'. Has BrandModelSeeder run?");

            Guid ModelId(string slug) => models.TryGetValue(slug, out var model)
                ? model.Id
                : throw new InvalidOperationException(
                    $"Demo advertisement data references unknown model slug '{slug}'. Has BrandModelSeeder run?");

            Advertisement Ad(
                string title,
                string description,
                int year,
                decimal price,
                int engineCapacity,
                FuelType fuelType,
                TransmissionType transmissionType,
                string citySlug,
                string brandSlug,
                string modelSlug,
                string contactName,
                string contactPhone,
                string contactEmail) => new()
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Description = description,
                    Year = year,
                    Price = price,
                    EngineCapacity = engineCapacity,
                    FuelType = fuelType,
                    TransmissionType = transmissionType,
                    VehicleCondition = VehicleCondition.Used,
                    CityId = CityId(citySlug),
                    BrandId = BrandId(brandSlug),
                    ModelId = ModelId(modelSlug),
                    ContactName = contactName,
                    ContactPhone = contactPhone,
                    ContactEmail = contactEmail,
                    SellerId = sellerId,
                    // The entity default is AdStatus.Pending, which would leave the whole
                    // demo set unpublished. Active is the published state.
                    Status = AdStatus.Active,
                    CreatedAt = DateTime.UtcNow
                    // ReviewByAdminId / ReviewedAt stay null: no admin user is seeded, and
                    // the FK would reject a fabricated GUID.
                };

            var advertisements = new List<Advertisement>
            {
                Ad("Toyota Corolla 2017 – Well Maintained",
                   "Single owner Toyota Corolla, full service history, excellent condition.",
                   2017, 7350000, 1800, FuelType.Petrol, TransmissionType.Automatic,
                   "colombo", "toyota", "corolla",
                   "Nimal Perera", "0771234567", "nimal@gmail.com"),

                Ad("Honda Civic 2019 – Sport Edition",
                   "Low mileage Civic, original paint, no accidents.",
                   2019, 8850000, 1500, FuelType.Petrol, TransmissionType.Automatic,
                   "gampaha", "honda", "civic",
                   "Kasun Silva", "0719876543", "kasun@gmail.com"),

                Ad("BMW 320i M Sport 2018",
                   "Luxury sedan, M Sport package, excellent performance.",
                   2018, 14200000, 2000, FuelType.Petrol, TransmissionType.Automatic,
                   "kandy", "bmw", "320i",
                   "Ruwan Jayasuriya", "0752233445", "ruwan@gmail.com"),

                Ad("Mercedes-Benz C-Class 2016",
                   "German luxury with smooth ride and premium interior.",
                   2016, 12500000, 2000, FuelType.Petrol, TransmissionType.Automatic,
                   "matara", "mercedes-benz", "c-class",
                   "Sunil Fernando", "0785566778", "sunil@gmail.com"),

                Ad("Toyota Prius Hybrid 2015",
                   "Fuel efficient hybrid, perfect for daily commuting.",
                   2015, 6800000, 1800, FuelType.Hybrid, TransmissionType.Automatic,
                   "galle", "toyota", "prius",
                   "Ajith Kumara", "0768899001", "ajith@gmail.com"),

                Ad("Nissan X-Trail 2017 – Family SUV",
                   "Spacious SUV with excellent comfort and safety.",
                   2017, 9200000, 2000, FuelType.Petrol, TransmissionType.Automatic,
                   "kantale", "nissan", "x-trail",
                   "Imran Ahamed", "0723344556", "imran@gmail.com"),

                Ad("Toyota Land Cruiser 2014 – Off Road Beast",
                   "Powerful 4WD, perfect for long distance and off-road.",
                   2014, 24500000, 4500, FuelType.Diesel, TransmissionType.Automatic,
                   "anuradhapura", "toyota", "land-cruiser",
                   "Mahesh Bandara", "0701122334", "mahesh@gmail.com"),

                Ad("Honda Accord 2018 – Executive Sedan",
                   "Luxury and comfort combined. Full option, very smooth drive.",
                   2018, 10200000, 2000, FuelType.Petrol, TransmissionType.Automatic,
                   "moratuwa", "honda", "accord",
                   "Tharindu Madushan", "0714455667", "tharindu@gmail.com"),

                Ad("Nissan Navara 2016 – Double Cab",
                   "Well maintained pickup, ideal for work and family.",
                   2016, 9800000, 2500, FuelType.Diesel, TransmissionType.Manual,
                   "kegalle", "nissan", "navara",
                   "Sampath Wijesinghe", "0779988776", "sampath@gmail.com"),

                Ad("BMW X5 2017 – Luxury SUV",
                   "Premium SUV with excellent performance and comfort.",
                   2017, 18500000, 3000, FuelType.Diesel, TransmissionType.Automatic,
                   "kalutara", "bmw", "x5",
                   "Chaminda Perera", "0783344556", "chaminda@gmail.com"),

                Ad("Mercedes-Benz E-Class 2015",
                   "Elegant executive car, premium leather interior.",
                   2015, 13800000, 2000, FuelType.Petrol, TransmissionType.Automatic,
                   "kandy", "mercedes-benz", "e-class",
                   "Dinesh Rathnayake", "0761122334", "dinesh@gmail.com"),

                Ad("Toyota Corolla Hybrid 2019",
                   "Excellent fuel economy, perfect city vehicle.",
                   2019, 8250000, 1800, FuelType.Hybrid, TransmissionType.Automatic,
                   "colombo", "toyota", "corolla",
                   "Roshan Fernando", "0756677889", "roshan@gmail.com"),

                Ad("Nissan Sunny 2014 – Budget Friendly",
                   "Reliable daily car, low maintenance cost.",
                   2014, 4500000, 1500, FuelType.Petrol, TransmissionType.Manual,
                   "negombo", "nissan", "sunny",
                   "Isuru Silva", "0709988776", "isuru@gmail.com"),

                Ad("BMW M3 2016 – Performance Beast",
                   "High performance sports sedan, well maintained.",
                   2016, 22500000, 3000, FuelType.Petrol, TransmissionType.Automatic,
                   "dehiwala-mount-lavinia", "bmw", "m3",
                   "Shehan Wickramasinghe", "0724455667", "shehan@gmail.com"),

                Ad("Mercedes-Benz G-Wagon 2013",
                   "Iconic luxury SUV with strong road presence.",
                   2013, 29500000, 5000, FuelType.Petrol, TransmissionType.Automatic,
                   "galle", "mercedes-benz", "g-wagon",
                   "Fazil Rahman", "0712233445", "fazil@gmail.com")
            };

            dbContext.Advertisements.AddRange(advertisements);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Seeded {Count} demo advertisements.", advertisements.Count);
        }
    }
}
