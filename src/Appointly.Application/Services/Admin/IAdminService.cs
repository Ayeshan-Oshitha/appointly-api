using Appointly.Application.Services.Admin.Contracts;

namespace Appointly.Application.Services.Admin
{
    public interface IAdminService
    {
        Task<List<UserResponse>> GetAllUsers();
        Task<bool> PromoteToAdmin(Guid userId);
        Task<bool> PromoteToSeller(Guid userId);
    }
}
