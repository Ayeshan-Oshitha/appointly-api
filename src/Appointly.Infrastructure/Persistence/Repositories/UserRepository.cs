using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Admin.Contracts;
using Appointly.Domain.Common.Constants;
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

                var roleResult = await _userManager.AddToRoleAsync(identityUser, Roles.User);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(roleResult.Errors.First().Description);
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
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                return null;
            }

            return await _dbContext.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
        }

        public async Task<User> IsPasswordValid(string email, string password)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(identityUser, password);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return user;
        }

        public async Task<UserResponse> GetUserProfileByIdAsync(Guid userId)
        {
            var domainUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (domainUser == null)
            {
                throw new NotFoundException("User not found.");
            }

            var identityUser = await _userManager.FindByIdAsync(domainUser.IdentityUserId.ToString());

            if (identityUser == null)
            {
                throw new NotFoundException("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);

            return new UserResponse
            {
                Id = domainUser.Id,
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
                Email = identityUser.Email,
                PhoneNumber = identityUser.PhoneNumber,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> UserExistsAsync(Guid userId)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
