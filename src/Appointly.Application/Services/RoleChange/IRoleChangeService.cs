using Appointly.Application.Services.RoleChange.Contracts;
using Appointly.Domain.Entities;

namespace Appointly.Application.Services.RoleChange
{
    public interface IRoleChangeService
    {
        Task AddRoleChangeRequest(Guid userId, string newRole);
        Task<List<RoleChangeRequest>> GetAllRequests(RoleChangeRequestQuery query);
    }
}
