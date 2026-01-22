using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.Admin.Contracts;
using Appointly.Domain.Common.Enum;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IRoleChangeRepository _roleChangeRepository;
        private readonly ICurrentUser _currentUser;
        public AdminService(IAdminRepository adminRepository, IRoleChangeRepository roleChangeRepository, ICurrentUser currentUser)
        {
            _adminRepository = adminRepository;
            _roleChangeRepository = roleChangeRepository;
            _currentUser = currentUser;
        }
        public async Task<List<UserResponse>> GetAllUsers()
        {
            return await _adminRepository.GetAllUsersAsync();
        }

        public async Task<bool> PromoteToAdmin(Guid userId)
        {
            return await _adminRepository.PromoteToAdminAsync(userId);
        }

        public async Task<bool> PromoteToSeller(Guid userId, Guid changeRoleRequestId)
        {
            var request = await _roleChangeRepository.GetRoleChangeRequestByIdAsync(changeRoleRequestId);

            if(request == null)
            {
                throw new NotFoundException("Not found the request");
            }

            if(request.Status != RoleRequestTypes.Pending)
            {
                throw new BadRequestException("The request is already processed");
            }

            if(request.UserId != userId)
            {
                throw new BadRequestException("The request does not belong to the specified user");
            }

            var currentUserId = _currentUser.Id;

            return await _adminRepository.PromoteToSellerAsync(userId, changeRoleRequestId, currentUserId);
        }
    }
}
