using MotorHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MotorHub.Infrastructure.Persistence.Seeders
{
    public static class LocationSeeder
    {
        public static async Task SeedAsync(MotorHubDbContext dbContext)
        {

            // 1. Provinces
            if (!await dbContext.Provinces.AnyAsync())
            {
                var provinces = new List<Province>
                {
                    new() { Id = Guid.NewGuid(), Name = "Western", Slug = "western" },
                    new() { Id = Guid.NewGuid(), Name = "Central", Slug = "central" },
                    new() { Id = Guid.NewGuid(), Name = "Southern", Slug = "southern" },
                    new() { Id = Guid.NewGuid(), Name = "Northern", Slug = "northern" },
                    new() { Id = Guid.NewGuid(), Name = "Eastern", Slug = "eastern" },
                    new() { Id = Guid.NewGuid(), Name = "North Western", Slug = "north-western" },
                    new() { Id = Guid.NewGuid(), Name = "North Central", Slug = "north-central" },
                    new() { Id = Guid.NewGuid(), Name = "Uva", Slug = "uva" },
                    new() { Id = Guid.NewGuid(), Name = "Sabaragamuwa", Slug = "sabaragamuwa" }
                };

                dbContext.Provinces.AddRange(provinces);
                await dbContext.SaveChangesAsync();
            }

            // 2. Districts
            if (!await dbContext.Districts.AnyAsync())
            {
                var provincesDict = await dbContext.Provinces.ToDictionaryAsync(p => p.Slug);

                var districts = new List<District>
                {
                    // Western
                    new() { Id = Guid.NewGuid(), Name = "Colombo", Slug = "colombo", ProvinceId = provincesDict["western"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Gampaha", Slug = "gampaha", ProvinceId = provincesDict["western"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kalutara", Slug = "kalutara", ProvinceId = provincesDict["western"].Id },

                    // Central
                    new() { Id = Guid.NewGuid(), Name = "Kandy", Slug = "kandy", ProvinceId = provincesDict["central"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Matale", Slug = "matale", ProvinceId = provincesDict["central"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Nuwara Eliya", Slug = "nuwara-eliya", ProvinceId = provincesDict["central"].Id },

                    // Southern
                    new() { Id = Guid.NewGuid(), Name = "Galle", Slug = "galle", ProvinceId = provincesDict["southern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Matara", Slug = "matara", ProvinceId = provincesDict["southern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Hambantota", Slug = "hambantota", ProvinceId = provincesDict["southern"].Id },

                    // Northern
                    new() { Id = Guid.NewGuid(), Name = "Jaffna", Slug = "jaffna", ProvinceId = provincesDict["northern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kilinochchi", Slug = "kilinochchi", ProvinceId = provincesDict["northern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Mannar", Slug = "mannar", ProvinceId = provincesDict["northern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Vavuniya", Slug = "vavuniya", ProvinceId = provincesDict["northern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Mullaitivu", Slug = "mullaitivu", ProvinceId = provincesDict["northern"].Id },

                    // Eastern
                    new() { Id = Guid.NewGuid(), Name = "Trincomalee", Slug = "trincomalee", ProvinceId = provincesDict["eastern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Batticaloa", Slug = "batticaloa", ProvinceId = provincesDict["eastern"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Ampara", Slug = "ampara", ProvinceId = provincesDict["eastern"].Id },

                    // North Western
                    new() { Id = Guid.NewGuid(), Name = "Kurunegala", Slug = "kurunegala", ProvinceId = provincesDict["north-western"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Puttalam", Slug = "puttalam", ProvinceId = provincesDict["north-western"].Id },

                    // North Central
                    new() { Id = Guid.NewGuid(), Name = "Anuradhapura", Slug = "anuradhapura", ProvinceId = provincesDict["north-central"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Polonnaruwa", Slug = "polonnaruwa", ProvinceId = provincesDict["north-central"].Id },

                    // Uva
                    new() { Id = Guid.NewGuid(), Name = "Badulla", Slug = "badulla", ProvinceId = provincesDict["uva"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Monaragala", Slug = "monaragala", ProvinceId = provincesDict["uva"].Id },

                    // Sabaragamuwa
                    new() { Id = Guid.NewGuid(), Name = "Ratnapura", Slug = "ratnapura", ProvinceId = provincesDict["sabaragamuwa"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kegalle", Slug = "kegalle", ProvinceId = provincesDict["sabaragamuwa"].Id }
                };

                dbContext.Districts.AddRange(districts);
                await dbContext.SaveChangesAsync();
            }

            // 3. Cities
            if (!await dbContext.Cities.AnyAsync())
            {
                var d = await dbContext.Districts.ToDictionaryAsync(d => d.Slug);

                var cities = new List<City>
                {
                    // --- Colombo ---
                    new() { Id = Guid.NewGuid(), Name = "Colombo", Slug = "colombo", ProvinceId = d["colombo"].ProvinceId, DistrictId = d["colombo"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Dehiwala-Mount Lavinia", Slug = "dehiwala-mount-lavinia", ProvinceId = d["colombo"].ProvinceId, DistrictId = d["colombo"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Moratuwa", Slug = "moratuwa", ProvinceId = d["colombo"].ProvinceId, DistrictId = d["colombo"].Id },

                    // --- Gampaha ---
                    new() { Id = Guid.NewGuid(), Name = "Negombo", Slug = "negombo", ProvinceId = d["gampaha"].ProvinceId, DistrictId = d["gampaha"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Gampaha", Slug = "gampaha", ProvinceId = d["gampaha"].ProvinceId, DistrictId = d["gampaha"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Ja-Ela", Slug = "ja-ela", ProvinceId = d["gampaha"].ProvinceId, DistrictId = d["gampaha"].Id },

                    // --- Kalutara ---
                    new() { Id = Guid.NewGuid(), Name = "Kalutara", Slug = "kalutara", ProvinceId = d["kalutara"].ProvinceId, DistrictId = d["kalutara"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Panadura", Slug = "panadura", ProvinceId = d["kalutara"].ProvinceId, DistrictId = d["kalutara"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Beruwala", Slug = "beruwala", ProvinceId = d["kalutara"].ProvinceId, DistrictId = d["kalutara"].Id },

                    // --- Kandy ---
                    new() { Id = Guid.NewGuid(), Name = "Kandy", Slug = "kandy", ProvinceId = d["kandy"].ProvinceId, DistrictId = d["kandy"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Katugastota", Slug = "katugastota", ProvinceId = d["kandy"].ProvinceId, DistrictId = d["kandy"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Peradeniya", Slug = "peradeniya", ProvinceId = d["kandy"].ProvinceId, DistrictId = d["kandy"].Id },

                    // --- Matale ---
                    new() { Id = Guid.NewGuid(), Name = "Matale", Slug = "matale", ProvinceId = d["matale"].ProvinceId, DistrictId = d["matale"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Dambulla", Slug = "dambulla", ProvinceId = d["matale"].ProvinceId, DistrictId = d["matale"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Galewela", Slug = "galewela", ProvinceId = d["matale"].ProvinceId, DistrictId = d["matale"].Id },

                    // --- Nuwara Eliya ---
                    new() { Id = Guid.NewGuid(), Name = "Nuwara Eliya", Slug = "nuwara-eliya", ProvinceId = d["nuwara-eliya"].ProvinceId, DistrictId = d["nuwara-eliya"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Hatton", Slug = "hatton", ProvinceId = d["nuwara-eliya"].ProvinceId, DistrictId = d["nuwara-eliya"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Talawakele", Slug = "talawakele", ProvinceId = d["nuwara-eliya"].ProvinceId, DistrictId = d["nuwara-eliya"].Id },

                    // --- Galle ---
                    new() { Id = Guid.NewGuid(), Name = "Galle", Slug = "galle", ProvinceId = d["galle"].ProvinceId, DistrictId = d["galle"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Hikkaduwa", Slug = "hikkaduwa", ProvinceId = d["galle"].ProvinceId, DistrictId = d["galle"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Ambalangoda", Slug = "ambalangoda", ProvinceId = d["galle"].ProvinceId, DistrictId = d["galle"].Id },

                    // --- Matara ---
                    new() { Id = Guid.NewGuid(), Name = "Matara", Slug = "matara", ProvinceId = d["matara"].ProvinceId, DistrictId = d["matara"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Weligama", Slug = "weligama", ProvinceId = d["matara"].ProvinceId, DistrictId = d["matara"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Akuressa", Slug = "akuressa", ProvinceId = d["matara"].ProvinceId, DistrictId = d["matara"].Id },

                    // --- Hambantota ---
                    new() { Id = Guid.NewGuid(), Name = "Hambantota", Slug = "hambantota", ProvinceId = d["hambantota"].ProvinceId, DistrictId = d["hambantota"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Tangalle", Slug = "tangalle", ProvinceId = d["hambantota"].ProvinceId, DistrictId = d["hambantota"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Tissamaharama", Slug = "tissamaharama", ProvinceId = d["hambantota"].ProvinceId, DistrictId = d["hambantota"].Id },

                    // --- Jaffna ---
                    new() { Id = Guid.NewGuid(), Name = "Jaffna", Slug = "jaffna", ProvinceId = d["jaffna"].ProvinceId, DistrictId = d["jaffna"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Chavakachcheri", Slug = "chavakachcheri", ProvinceId = d["jaffna"].ProvinceId, DistrictId = d["jaffna"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Point Pedro", Slug = "point-pedro", ProvinceId = d["jaffna"].ProvinceId, DistrictId = d["jaffna"].Id },

                    // --- Kilinochchi ---
                    new() { Id = Guid.NewGuid(), Name = "Kilinochchi", Slug = "kilinochchi", ProvinceId = d["kilinochchi"].ProvinceId, DistrictId = d["kilinochchi"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Pallai", Slug = "pallai", ProvinceId = d["kilinochchi"].ProvinceId, DistrictId = d["kilinochchi"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Paranthan", Slug = "paranthan", ProvinceId = d["kilinochchi"].ProvinceId, DistrictId = d["kilinochchi"].Id },

                    // --- Mannar ---
                    new() { Id = Guid.NewGuid(), Name = "Mannar", Slug = "mannar", ProvinceId = d["mannar"].ProvinceId, DistrictId = d["mannar"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Pesalai", Slug = "pesalai", ProvinceId = d["mannar"].ProvinceId, DistrictId = d["mannar"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Madhu", Slug = "madhu", ProvinceId = d["mannar"].ProvinceId, DistrictId = d["mannar"].Id },

                    // --- Vavuniya ---
                    new() { Id = Guid.NewGuid(), Name = "Vavuniya", Slug = "vavuniya", ProvinceId = d["vavuniya"].ProvinceId, DistrictId = d["vavuniya"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Nedunkeni", Slug = "nedunkeni", ProvinceId = d["vavuniya"].ProvinceId, DistrictId = d["vavuniya"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Omanthai", Slug = "omanthai", ProvinceId = d["vavuniya"].ProvinceId, DistrictId = d["vavuniya"].Id },

                    // --- Mullaitivu ---
                    new() { Id = Guid.NewGuid(), Name = "Mullaitivu", Slug = "mullaitivu", ProvinceId = d["mullaitivu"].ProvinceId, DistrictId = d["mullaitivu"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Puthukudiyiruppu", Slug = "puthukudiyiruppu", ProvinceId = d["mullaitivu"].ProvinceId, DistrictId = d["mullaitivu"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Oddusuddan", Slug = "oddusuddan", ProvinceId = d["mullaitivu"].ProvinceId, DistrictId = d["mullaitivu"].Id },

                    // --- Trincomalee ---
                    new() { Id = Guid.NewGuid(), Name = "Trincomalee", Slug = "trincomalee", ProvinceId = d["trincomalee"].ProvinceId, DistrictId = d["trincomalee"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kinniya", Slug = "kinniya", ProvinceId = d["trincomalee"].ProvinceId, DistrictId = d["trincomalee"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kantale", Slug = "kantale", ProvinceId = d["trincomalee"].ProvinceId, DistrictId = d["trincomalee"].Id },

                    // --- Batticaloa ---
                    new() { Id = Guid.NewGuid(), Name = "Batticaloa", Slug = "batticaloa", ProvinceId = d["batticaloa"].ProvinceId, DistrictId = d["batticaloa"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kattankudy", Slug = "kattankudy", ProvinceId = d["batticaloa"].ProvinceId, DistrictId = d["batticaloa"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Eravur", Slug = "eravur", ProvinceId = d["batticaloa"].ProvinceId, DistrictId = d["batticaloa"].Id },

                    // --- Ampara ---
                    new() { Id = Guid.NewGuid(), Name = "Ampara", Slug = "ampara", ProvinceId = d["ampara"].ProvinceId, DistrictId = d["ampara"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kalmunai", Slug = "kalmunai", ProvinceId = d["ampara"].ProvinceId, DistrictId = d["ampara"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Sainthamaruthu", Slug = "sainthamaruthu", ProvinceId = d["ampara"].ProvinceId, DistrictId = d["ampara"].Id },

                    // --- Kurunegala ---
                    new() { Id = Guid.NewGuid(), Name = "Kurunegala", Slug = "kurunegala", ProvinceId = d["kurunegala"].ProvinceId, DistrictId = d["kurunegala"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kuliyapitiya", Slug = "kuliyapitiya", ProvinceId = d["kurunegala"].ProvinceId, DistrictId = d["kurunegala"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Narammala", Slug = "narammala", ProvinceId = d["kurunegala"].ProvinceId, DistrictId = d["kurunegala"].Id },

                    // --- Puttalam ---
                    new() { Id = Guid.NewGuid(), Name = "Puttalam", Slug = "puttalam", ProvinceId = d["puttalam"].ProvinceId, DistrictId = d["puttalam"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Chilaw", Slug = "chilaw", ProvinceId = d["puttalam"].ProvinceId, DistrictId = d["puttalam"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Wennappuwa", Slug = "wennappuwa", ProvinceId = d["puttalam"].ProvinceId, DistrictId = d["puttalam"].Id },

                    // --- Anuradhapura ---
                    new() { Id = Guid.NewGuid(), Name = "Anuradhapura", Slug = "anuradhapura", ProvinceId = d["anuradhapura"].ProvinceId, DistrictId = d["anuradhapura"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kekirawa", Slug = "kekirawa", ProvinceId = d["anuradhapura"].ProvinceId, DistrictId = d["anuradhapura"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Medawachchiya", Slug = "medawachchiya", ProvinceId = d["anuradhapura"].ProvinceId, DistrictId = d["anuradhapura"].Id },

                    // --- Polonnaruwa ---
                    new() { Id = Guid.NewGuid(), Name = "Polonnaruwa", Slug = "polonnaruwa", ProvinceId = d["polonnaruwa"].ProvinceId, DistrictId = d["polonnaruwa"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Kaduruwela", Slug = "kaduruwela", ProvinceId = d["polonnaruwa"].ProvinceId, DistrictId = d["polonnaruwa"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Hingurakgoda", Slug = "hingurakgoda", ProvinceId = d["polonnaruwa"].ProvinceId, DistrictId = d["polonnaruwa"].Id },

                    // --- Badulla ---
                    new() { Id = Guid.NewGuid(), Name = "Badulla", Slug = "badulla", ProvinceId = d["badulla"].ProvinceId, DistrictId = d["badulla"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Bandarawela", Slug = "bandarawela", ProvinceId = d["badulla"].ProvinceId, DistrictId = d["badulla"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Ella", Slug = "ella", ProvinceId = d["badulla"].ProvinceId, DistrictId = d["badulla"].Id },

                    // --- Monaragala ---
                    new() { Id = Guid.NewGuid(), Name = "Monaragala", Slug = "monaragala", ProvinceId = d["monaragala"].ProvinceId, DistrictId = d["monaragala"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Bibile", Slug = "bibile", ProvinceId = d["monaragala"].ProvinceId, DistrictId = d["monaragala"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Wellawaya", Slug = "wellawaya", ProvinceId = d["monaragala"].ProvinceId, DistrictId = d["monaragala"].Id },

                    // --- Ratnapura ---
                    new() { Id = Guid.NewGuid(), Name = "Ratnapura", Slug = "ratnapura", ProvinceId = d["ratnapura"].ProvinceId, DistrictId = d["ratnapura"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Balangoda", Slug = "balangoda", ProvinceId = d["ratnapura"].ProvinceId, DistrictId = d["ratnapura"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Embilipitiya", Slug = "embilipitiya", ProvinceId = d["ratnapura"].ProvinceId, DistrictId = d["ratnapura"].Id },

                    // --- Kegalle ---
                    new() { Id = Guid.NewGuid(), Name = "Kegalle", Slug = "kegalle", ProvinceId = d["kegalle"].ProvinceId, DistrictId = d["kegalle"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Mawanella", Slug = "mawanella", ProvinceId = d["kegalle"].ProvinceId, DistrictId = d["kegalle"].Id },
                    new() { Id = Guid.NewGuid(), Name = "Rambukkana", Slug = "rambukkana", ProvinceId = d["kegalle"].ProvinceId, DistrictId = d["kegalle"].Id }
                };

                dbContext.Cities.AddRange(cities);
                await dbContext.SaveChangesAsync();
            }



        }
    }
}
