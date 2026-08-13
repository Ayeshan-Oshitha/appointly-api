
using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.DTOs.RoleChange;
using Appointly.Domain.Common.Constants;
using Appointly.Domain.Entities;
using Appointly.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace Appointly.Application.Services.RoleChange
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
            if (!Roles.All.Contains(request.RequestedRole))
            {
                throw new BadRequestException("Invalid role specified.");
            }

            var user = await _userRepository.UserExistsAsync(request.UserId);

            if (!user)
            {
                throw new NotFoundException("User not found.");
            }


            await _roleChangeRepository.AddRoleChangeRequestAsync(request.UserId, request.RequestedRole);
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
