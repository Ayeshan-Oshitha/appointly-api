using Appointly.Application.Services.Admin.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Services.Admin
{
    public interface IAdminService
    {
        Task<List<UserResponse>> GetAllUsers();
        Task<bool> PromoteToAdmin(Guid userId, Guid changeRoleRequestId);
        Task<bool> PromoteToSeller(Guid userId, Guid changeRoleRequestId);
        Task<bool> RejectPromoteRequest(Guid userId, Guid changeRoleRequestId, string? rejectReason);
        Task<Advertisement> ApproveAdvertisment(Guid AdvertismentId);
        Task<Advertisement> RejectAdvertisment(Guid AdvertismentId, string? reason);
        Task<Advertisement> BlockAdvertisment(Guid AdvertismentId);
        Task<Advertisement> UndoAdvertismentReview(Guid AdvertismentId);
    }
}
