using MotorHub.Application.DTOs.Admin;
using MotorHub.Domain.Entities;

namespace MotorHub.Application.Common.Interfaces.Persistence
{
    public interface IAdminRepository
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<bool> PromoteToAdminAsync(Guid userId, Guid changeRoleRequestId, Guid currrentUserId);
        Task<bool> PromoteToSellerAsync(Guid userId, Guid changeRoleRequestId, Guid currrentUserId);
        Task<bool> RejectPromoteRequestAsync(Guid changeRoleRequestId, Guid currrentUserId, string? rejectReason);
        Task<RoleChangeRequest?> GetExistingPendingRequestsByUserIdAsync(Guid userId);

        Task<Advertisement> ApproveAdvertisementAsync(Guid AdvertisementId, Guid currentUserId);
        Task<Advertisement> RejectAdvertisementAsync(Guid AdvertisementId, Guid currentUserId, string? reason);
        Task<Advertisement> BlockAdvertisementAsync(Guid AdvertisementId, Guid currentUserId, string? reason);
        Task<Advertisement> UndoAdvertisementReviewAsync(Guid AdvertisementId);
    }
}
