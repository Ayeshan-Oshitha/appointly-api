
using Appointly.Application.Services.Admin.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> AddUserAsync(string firstName, string lastName, string email, string password, string phoneNumber);
        Task<User> IsPasswordValid(string email, string password);
        Task<UserResponse> GetUserProfileByIdAsync(Guid userId);
        Task<bool> UserExistsAsync(Guid userId);
    }
}
