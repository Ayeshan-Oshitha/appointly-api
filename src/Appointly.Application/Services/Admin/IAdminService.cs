using Appointly.Application.Services.Admin.Contracts;

namespace Appointly.Application.Services.Admin
{
    public interface IAdminService
    {
        Task<List<UserResponse>> GetAllUsers();
    }
}
