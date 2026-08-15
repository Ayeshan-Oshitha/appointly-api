using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Admin;
using MotorHub.Domain.Common.Constants;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MotorHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MotorHub.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MotorHubDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(MotorHubDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<User> AddUserAsync(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            // Identity's CreateAsync/AddToRoleAsync each commit on their own. Without an
            // enclosing transaction a failure partway through leaves an Identity account with
            // no matching domain User: it can authenticate, but every lookup returns null and
            // the address is taken, so the account can't even be re-registered.
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

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
                    // Identity catches the duplicate before Postgres does, so this - not the
                    // UniqueViolation handler below - is the path a duplicate email takes.
                    if (result.Errors.Any(e => e.Code == nameof(IdentityErrorDescriber.DuplicateUserName)
                        || e.Code == nameof(IdentityErrorDescriber.DuplicateEmail)))
                    {
                        throw new ConflictException("User with this email already exists.");
                    }

                    throw new BadRequestException(string.Join("; ", result.Errors.Select(e => e.Description)));
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

                await _dbContext.DomainUsers.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return user;
            }

            // Handle unique constraint violation for email / DbTransaction error handling
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await transaction.RollbackAsync();
                throw new ConflictException("User with this email already exists.");

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser == null)
            {
                return null;
            }

            return await _dbContext.DomainUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
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

            var user = await _dbContext.DomainUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return user;
        }

        public async Task<UserResponseDto> GetUserProfileByIdAsync(Guid userId)
        {
            var domainUser = await _dbContext.DomainUsers.FirstOrDefaultAsync(u => u.Id == userId);

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

            return new UserResponseDto
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
            return await _dbContext.DomainUsers.AnyAsync(u => u.Id == userId);
        }
    }
}
