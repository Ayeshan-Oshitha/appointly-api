using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IuserRepository
    {
        private readonly AppointlyDbContext _dbContext;
        public UserRepository(AppointlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> AddUserAsync(string firstName, string lastName, string email, string password)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = password
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return user;
        }
    }
}
