using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppointlyDbContext _dbContext;
        public UserRepository(AppointlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> AddUserAsync(string firstName, string lastName, string email, string password)
        {
            try
            {
                var user = new User(
                firstName,
                lastName,
                email,
                password
            );
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }

            // Handle unique constraint violation for email / DbTransaction error handling
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new ConflictException("User with this email already exists.");
                
            }
            
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower());
            return user;
        }
    }
}
