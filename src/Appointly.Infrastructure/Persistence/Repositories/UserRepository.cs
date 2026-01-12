using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using Appointly.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Appointly.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppointlyDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(AppointlyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<User> AddUserAsync(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            try
            {

                var identityUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = phoneNumber
                };

                var result = await _userManager.CreateAsync(identityUser, password);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                var user = new User(
                identityUser.Id,
                firstName,
                lastName,
                phoneNumber
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
            //var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower());
            //return user;
            throw new NotImplementedException();
        }
    }
}
