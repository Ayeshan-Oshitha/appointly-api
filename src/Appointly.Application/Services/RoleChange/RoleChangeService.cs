
using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Services.RoleChange.Contracts;
using Appointly.Domain.Common.Constants;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.RoleChange
{
    public class RoleChangeService : IRoleChangeService
    {
        private readonly IRoleChangeRepository _roleChangeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        public RoleChangeService(IRoleChangeRepository roleChangeRepository, IUserRepository userRepository, ICurrentUser currentUser)
        {
            _roleChangeRepository = roleChangeRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }
        public async Task AddRoleChangeRequest(Guid userId, string newRole)
        {
            if (!Roles.All.Contains(newRole))
            {
                throw new BadRequestException("Invalid role specified.");
            }

            var user = await _userRepository.UserExistsAsync(userId);

            if (!user)
            {
                throw new NotFoundException("User not found.");
            }


            await _roleChangeRepository.AddRoleChangeRequestAsync(userId, newRole);
        }

        public async Task<List<RoleChangeRequest>> GetAllRequests(RoleChangeRequestQuery query)
        {
            return await _roleChangeRepository.GetAllRequestsAsync(query);
        }

        public async Task DeleteRoleChangeRequest(Guid id)
        {
            var existingRequest = await _roleChangeRepository.GetRoleChangeRequestByIdAsync(id);

            if (existingRequest == null)
            {
                throw new NotFoundException("Role change request not found.");
            }

            if(existingRequest.UserId != _currentUser.Id)
            {
                throw new ForbiddenException("You do not have permission to delete this role change request. " +
                    "Only The request Creator can delete this");
            }

            var deleted = await _roleChangeRepository.DeleteRoleChangeRequestAsync(id);

            if (!deleted)
            {
                throw new Exception("Failed to delete role change request.");
            }
        }
    }
}
