using Appointly.Domain.Common.Enum;
using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Seeders
{
    public static class AdvertismentSeeder
    {
        public static async Task SeedAsync(AppointlyDbContext dbContext)
        {
            if (!await dbContext.Advertisements.AnyAsync())
            {
                var advertisments = new List<Advertisement>
                {
                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Toyota Corolla 2017 – Well Maintained",
                        Description = "Single owner Toyota Corolla, full service history, excellent condition.",
                        Year = 2017,
                        Price = 7350000,
                        EngineCapacity = 1800,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("ac6b360a-a06e-4d51-ac43-525b46743a1b"), // Colombo
                        BrandId = Guid.Parse("8bd27fcf-275d-4397-be67-5a8986d9e24b"), // Toyota
                        ModelId = Guid.Parse("8b62b639-6213-412b-962f-d8635c1869ca"), // Corolla
                        ContactName = "Nimal Perera",
                        ContactPhone = "0771234567",
                        ContactEmail = "nimal@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Honda Civic 2019 – Sport Edition",
                        Description = "Low mileage Civic, original paint, no accidents.",
                        Year = 2019,
                        Price = 8850000,
                        EngineCapacity = 1500,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("8cdb9684-2306-4f97-99da-26acf99c291b"), // Gampaha
                        BrandId = Guid.Parse("ea38333f-a3ae-436b-a052-9e67ab4383a8"), // Honda
                        ModelId = Guid.Parse("d16ea5ee-3c78-4a43-a35d-82aa6bd97d48"), // Civic
                        ContactName = "Kasun Silva",
                        ContactPhone = "0719876543",
                        ContactEmail = "kasun@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "BMW 320i M Sport 2018",
                        Description = "Luxury sedan, M Sport package, excellent performance.",
                        Year = 2018,
                        Price = 14200000,
                        EngineCapacity = 2000,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("913cfe96-120f-4db9-8a89-ebd44a2fe0fa"), // Kandy
                        BrandId = Guid.Parse("c023208c-1763-4a41-b4cb-541910570156"), // BMW
                        ModelId = Guid.Parse("ba690cca-aaa8-436e-9e7b-5c858c403101"), // 320i
                        ContactName = "Ruwan Jayasuriya",
                        ContactPhone = "0752233445",
                        ContactEmail = "ruwan@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Mercedes-Benz C-Class 2016",
                        Description = "German luxury with smooth ride and premium interior.",
                        Year = 2016,
                        Price = 12500000,
                        EngineCapacity = 2000,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("2296d0bd-cf50-43b8-8aa7-006ef2ff5144"), // Matara
                        BrandId = Guid.Parse("27fd46fb-ba4a-43c7-8208-8b3041d9fb6a"), // Mercedes
                        ModelId = Guid.Parse("4686553d-ec02-4d86-b08e-8b92818eddcd"), // C-Class
                        ContactName = "Sunil Fernando",
                        ContactPhone = "0785566778",
                        ContactEmail = "sunil@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Toyota Prius Hybrid 2015",
                        Description = "Fuel efficient hybrid, perfect for daily commuting.",
                        Year = 2015,
                        Price = 6800000,
                        EngineCapacity = 1800,
                        FuelType = FuelType.Hybrid,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("f5c27c1e-2e7d-43d1-afd6-5ae5e63eaaa4"), // Galle
                        BrandId = Guid.Parse("8bd27fcf-275d-4397-be67-5a8986d9e24b"),
                        ModelId = Guid.Parse("b0b923e4-9f21-4e56-a630-548fd38a7f78"), // Prius
                        ContactName = "Ajith Kumara",
                        ContactPhone = "0768899001",
                        ContactEmail = "ajith@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Nissan X-Trail 2017 – Family SUV",
                        Description = "Spacious SUV with excellent comfort and safety.",
                        Year = 2017,
                        Price = 9200000,
                        EngineCapacity = 2000,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("8ef60418-7b57-4686-987f-39312017f515"), // Kantale
                        BrandId = Guid.Parse("26a88ffe-7d4f-4de2-a0ac-c3581a1f2a7b"), // Nissan
                        ModelId = Guid.Parse("72a04544-d250-428c-abb6-8b88da4072c6"), // X-Trail
                        ContactName = "Imran Ahamed",
                        ContactPhone = "0723344556",
                        ContactEmail = "imran@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Toyota Land Cruiser 2014 – Off Road Beast",
                        Description = "Powerful 4WD, perfect for long distance and off-road.",
                        Year = 2014,
                        Price = 24500000,
                        EngineCapacity = 4500,
                        FuelType = FuelType.Diesel,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("e9d0eb0f-40de-4cbf-b833-05c6f4dce1aa"), // Anuradhapura
                        BrandId = Guid.Parse("8bd27fcf-275d-4397-be67-5a8986d9e24b"),
                        ModelId = Guid.Parse("e7494b6f-1b1f-4ef2-a6e7-bca69fed5404"), // Land Cruiser
                        ContactName = "Mahesh Bandara",
                        ContactPhone = "0701122334",
                        ContactEmail = "mahesh@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Honda Accord 2018 – Executive Sedan",
                        Description = "Luxury and comfort combined. Full option, very smooth drive.",
                        Year = 2018,
                        Price = 10200000,
                        EngineCapacity = 2000,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("92913e32-a8de-490c-86f2-70e805e082fb"), // Moratuwa
                        BrandId = Guid.Parse("ea38333f-a3ae-436b-a052-9e67ab4383a8"), // Honda
                        ModelId = Guid.Parse("0659c79d-295b-4ce1-a3ca-fb06dc5acd28"), // Accord
                        ContactName = "Tharindu Madushan",
                        ContactPhone = "0714455667",
                        ContactEmail = "tharindu@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Nissan Navara 2016 – Double Cab",
                        Description = "Well maintained pickup, ideal for work and family.",
                        Year = 2016,
                        Price = 9800000,
                        EngineCapacity = 2500,
                        FuelType = FuelType.Diesel,
                        TransmissionType = TransmissionType.Manual,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("3a2e2e3c-6e0e-4836-867e-b006338fad30"), // Kegalle
                        BrandId = Guid.Parse("26a88ffe-7d4f-4de2-a0ac-c3581a1f2a7b"), // Nissan
                        ModelId = Guid.Parse("3ccb8746-eb90-4e55-a694-6c9ad6f309f7"), // Navara
                        ContactName = "Sampath Wijesinghe",
                        ContactPhone = "0779988776",
                        ContactEmail = "sampath@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "BMW X5 2017 – Luxury SUV",
                        Description = "Premium SUV with excellent performance and comfort.",
                        Year = 2017,
                        Price = 18500000,
                        EngineCapacity = 3000,
                        FuelType = FuelType.Diesel,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("f995175b-aa26-4374-ab6f-3abd7758a271"), // Kalutara
                        BrandId = Guid.Parse("c023208c-1763-4a41-b4cb-541910570156"), // BMW
                        ModelId = Guid.Parse("56395ac6-c798-49ef-9f08-3599aa09419c"), // X5
                        ContactName = "Chaminda Perera",
                        ContactPhone = "0783344556",
                        ContactEmail = "chaminda@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Mercedes-Benz E-Class 2015",
                        Description = "Elegant executive car, premium leather interior.",
                        Year = 2015,
                        Price = 13800000,
                        EngineCapacity = 2000,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("913cfe96-120f-4db9-8a89-ebd44a2fe0fa"), // Kandy
                        BrandId = Guid.Parse("27fd46fb-ba4a-43c7-8208-8b3041d9fb6a"), // Mercedes-Benz
                        ModelId = Guid.Parse("3fc0fa33-b711-4bee-a9bb-45b50598d79d"), // E-Class
                        ContactName = "Dinesh Rathnayake",
                        ContactPhone = "0761122334",
                        ContactEmail = "dinesh@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Toyota Corolla Hybrid 2019",
                        Description = "Excellent fuel economy, perfect city vehicle.",
                        Year = 2019,
                        Price = 8250000,
                        EngineCapacity = 1800,
                        FuelType = FuelType.Hybrid,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("ac6b360a-a06e-4d51-ac43-525b46743a1b"), // Colombo
                        BrandId = Guid.Parse("8bd27fcf-275d-4397-be67-5a8986d9e24b"), // Toyota
                        ModelId = Guid.Parse("8b62b639-6213-412b-962f-d8635c1869ca"), // Corolla
                        ContactName = "Roshan Fernando",
                        ContactPhone = "0756677889",
                        ContactEmail = "roshan@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Nissan Sunny 2014 – Budget Friendly",
                        Description = "Reliable daily car, low maintenance cost.",
                        Year = 2014,
                        Price = 4500000,
                        EngineCapacity = 1500,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Manual,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("bd92a50c-2cb3-4a8f-b6cf-4fdab9d8dde4"), // Negombo
                        BrandId = Guid.Parse("26a88ffe-7d4f-4de2-a0ac-c3581a1f2a7b"), // Nissan
                        ModelId = Guid.Parse("ff69ef96-92af-41a0-ab7a-ee7577776c99"), // Sunny
                        ContactName = "Isuru Silva",
                        ContactPhone = "0709988776",
                        ContactEmail = "isuru@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "BMW M3 2016 – Performance Beast",
                        Description = "High performance sports sedan, well maintained.",
                        Year = 2016,
                        Price = 22500000,
                        EngineCapacity = 3000,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("135d0284-ceff-4c16-8ef9-2aced7e33c69"), // Dehiwala
                        BrandId = Guid.Parse("c023208c-1763-4a41-b4cb-541910570156"), // BMW
                        ModelId = Guid.Parse("97be0af6-c6c6-41bf-b998-adc234c0f09c"), // M3
                        ContactName = "Shehan Wickramasinghe",
                        ContactPhone = "0724455667",
                        ContactEmail = "shehan@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    },

                    new Advertisement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Mercedes-Benz G-Wagon 2013",
                        Description = "Iconic luxury SUV with strong road presence.",
                        Year = 2013,
                        Price = 29500000,
                        EngineCapacity = 5000,
                        FuelType = FuelType.Petrol,
                        TransmissionType = TransmissionType.Automatic,
                        VehicleCondition = VehicleCondition.Used,
                        CityId = Guid.Parse("f5c27c1e-2e7d-43d1-afd6-5ae5e63eaaa4"), // Galle
                        BrandId = Guid.Parse("27fd46fb-ba4a-43c7-8208-8b3041d9fb6a"), // Mercedes-Benz
                        ModelId = Guid.Parse("fddf26ad-a6bc-494f-95f3-cc26b1cbaeae"), // G-Wagon
                        ContactName = "Fazil Rahman",
                        ContactPhone = "0712233445",
                        ContactEmail = "fazil@gmail.com",
                        SellerId = Guid.Parse("e30a0b97-cf20-49f7-86b5-b4afe065f247") // John Doe
                    }
                };

                dbContext.Advertisements.AddRange(advertisments);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
