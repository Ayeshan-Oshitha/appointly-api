using Appointly.Application.Services.Admin.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IAdminRepository
    {
        Task<List<UserResponse>> GetAllUsersAsync();
        Task<bool> PromoteToAdminAsync(Guid userId, Guid changeRoleRequestId, Guid currrentUserId);
        Task<bool> PromoteToSellerAsync(Guid userId, Guid changeRoleRequestId, Guid currrentUserId);
        Task<bool> RejectPromoteRequestAsync(Guid changeRoleRequestId, Guid currrentUserId, string? rejectReason);
        Task<RoleChangeRequest?> GetExistingPendingRequestsByUserIdAsync(Guid userId);

        Task<Advertisement> ApproveAdvertisementAsync(Guid AdvertismentId, Guid currentUserId);
        Task<Advertisement> RejectAdvertisementAsync(Guid AdvertismentId, Guid currentUserId, string? reason);
        Task<Advertisement> BlockAdvertisementAsync(Guid AdvertismentId, Guid currentUserId, string? reason);
        Task<Advertisement> UndoAdvertismentReviewAsync(Guid AdvertismentId);
    }
}
