using Appointly.Application.Services.Admin.Contracts;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IAdminRepository
    {
        Task<List<UserResponse>> GetAllUsersAsync();
    }
}
