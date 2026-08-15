
using MotorHub.Application.Common.CurrentUser;
using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.RoleChange;
using MotorHub.Domain.Common.Constants;
using MotorHub.Domain.Entities;
using MotorHub.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace MotorHub.Application.Services.RoleChange
{
    public class RoleChangeService : IRoleChangeService
    {
        private readonly IRoleChangeRepository _roleChangeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        public RoleChangeService(IRoleChangeRepository roleChangeRepository, IUserRepository userRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _roleChangeRepository = roleChangeRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task AddRoleChangeRequest(AddRoleChangeRequestDto request)
        {
            // Seller is the only self-requestable role. Admin and SuperAdmin must never be
            // reachable this way, otherwise the request queue becomes a privilege-escalation path.
            if (request.RequestedRole != Roles.Seller)
            {
                throw new BadRequestException("Only the Seller role can be requested.");
            }

            var userId = _currentUser.Id;

            var user = await _userRepository.UserExistsAsync(userId);

            if (!user)
            {
                throw new NotFoundException("User not found.");
            }


            await _roleChangeRepository.AddRoleChangeRequestAsync(userId, request.RequestedRole);
        }

        public async Task<List<RoleChangeResponseDto>> GetAllRequests(RoleChangeRequestQueryDto query)
        {
            var requests = await _roleChangeRepository.GetAllRequestsAsync(query);
            return _mapper.Map<List<RoleChangeResponseDto>>(requests);
        }

        public async Task DeleteRoleChangeRequest(Guid id)
        {
            var existingRequest = await _roleChangeRepository.GetRoleChangeRequestByIdAsync(id);

            if (existingRequest == null)
            {
                throw new NotFoundException("Role change request not found.");
            }

            if(existingRequest.UserId != _currentUser.Id && !_currentUser.IsAdmin())
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
