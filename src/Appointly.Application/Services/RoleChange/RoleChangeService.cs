
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Domain.Common.Constants;
using Appointly.Domain.Infrastructure.Exceptions;

namespace Appointly.Application.Services.RoleChange
{
    public class RoleChangeService : IRoleChangeService
    {
        private readonly IRoleChangeRepository _roleChangeRepository;
        private readonly IUserRepository _userRepository;
        public RoleChangeService(IRoleChangeRepository roleChangeRepository, IUserRepository userRepository)
        {
            _roleChangeRepository = roleChangeRepository;
            _userRepository = userRepository;
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
    }
}
