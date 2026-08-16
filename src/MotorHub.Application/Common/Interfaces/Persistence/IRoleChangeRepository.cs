using MotorHub.Application.DTOs.RoleChange;
using MotorHub.Domain.Entities;

namespace MotorHub.Application.Common.Interfaces.Persistence
{
    public interface IRoleChangeRepository
    {
        Task AddRoleChangeRequestAsync(Guid userId, string newRole);
        Task<List<RoleChangeRequest>> GetAllRequestsAsync(RoleChangeRequestQueryDto query);
        Task<RoleChangeRequest?> GetRoleChangeRequestByIdAsync(Guid id);
        Task<bool> DeleteRoleChangeRequestAsync(Guid id);
    }
}
