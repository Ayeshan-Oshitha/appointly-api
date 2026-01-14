using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Admin.Contracts;

namespace Appointly.Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<List<UserResponse>> GetAllUsers()
        {
            return await _adminRepository.GetAllUsersAsync();
        }
    }
}
