using Appointly.Application.Services.Admin.Contracts;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IAdminRepository
    {
        Task<List<UserResponse>> GetAllUsersAsync();
        Task<bool> PromoteToAdminAsync(Guid userId);
        Task<bool> PromoteToSellerAsync(Guid userId, Guid changeRoleRequestId, Guid currrentUserId);
    }
}
