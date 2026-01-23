using Appointly.Application.Services.RoleChange.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IRoleChangeRepository
    {
        Task AddRoleChangeRequestAsync(Guid userId, string newRole);
        Task<List<RoleChangeRequest>> GetAllRequestsAsync(RoleChangeRequestQuery query);
        Task<RoleChangeRequest?> GetRoleChangeRequestByIdAsync(Guid id);
        Task<bool> DeleteRoleChangeRequestAsync(Guid id);
    }
}
