using Appointly.Application.Services.Admin.Contracts;

namespace Appointly.Application.Services.Admin
{
    public interface IAdminService
    {
        Task<List<UserResponse>> GetAllUsers();
        Task<bool> PromoteToAdmin(Guid userId, Guid changeRoleRequestId);
        Task<bool> PromoteToSeller(Guid userId, Guid changeRoleRequestId);
        Task<bool> RejectPromoteRequest(Guid userId, Guid changeRoleRequestId, string? rejectReason);
    }
}
