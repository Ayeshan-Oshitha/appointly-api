using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class AdvertisementRepository : IAdvertismentRepository
    {
        private readonly AppointlyDbContext _dbContext;
        public AdvertisementRepository(AppointlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Advertisement> AddAdvertismentAsync(Advertisement advertisment)
        {
            _dbContext.Advertisements.Add(advertisment);
            await  _dbContext.SaveChangesAsync();
            return advertisment;
        }

        public async Task<Advertisement?> GetAdvertismentByIdAsync(Guid id)
        {
            return await _dbContext.Advertisements.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Advertisement>> GetAllAdvertismentsAsync()
        {
            return await _dbContext.Advertisements
                .AsNoTracking()
                .Include(a => a.Brand)
                .Include(a => a.Model)
                .Include(a => a.City)
                    .ThenInclude(c => c.District)
                .Include(a => a.City)
                    .ThenInclude(c => c.Province)
                .ToListAsync();
        }
    }
}
