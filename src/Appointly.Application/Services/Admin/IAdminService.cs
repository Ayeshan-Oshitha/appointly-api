using Appointly.Application.DTOs.Admin;
using Appointly.Application.DTOs.Advertisements;

namespace Appointly.Application.Services.Admin
{
    public interface IAdminService
    {
        Task<List<UserResponseDto>> GetAllUsers();
        Task<bool> PromoteToAdmin(Guid userId, Guid changeRoleRequestId);
        Task<bool> PromoteToSeller(Guid userId, Guid changeRoleRequestId);
        Task<bool> RejectPromoteRequest(Guid userId, Guid changeRoleRequestId, string? rejectReason);
        Task<AdvertisementResponseDto> ApproveAdvertisment(Guid AdvertismentId);
        Task<AdvertisementResponseDto> RejectAdvertisment(Guid AdvertismentId, string? reason);
        Task<AdvertisementResponseDto> BlockAdvertisment(Guid AdvertismentId);
        Task<AdvertisementResponseDto> UndoAdvertismentReview(Guid AdvertismentId);
    }
}
