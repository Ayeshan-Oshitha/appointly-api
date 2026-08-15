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
        Task<AdvertisementResponseDto> ApproveAdvertisement(Guid AdvertisementId);
        Task<AdvertisementResponseDto> RejectAdvertisement(Guid AdvertisementId, string? reason);
        Task<AdvertisementResponseDto> BlockAdvertisement(Guid AdvertisementId);
        Task<AdvertisementResponseDto> UndoAdvertisementReview(Guid AdvertisementId);
    }
}
