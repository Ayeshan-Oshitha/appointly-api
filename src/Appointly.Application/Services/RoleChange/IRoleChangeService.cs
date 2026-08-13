using Appointly.Application.DTOs.RoleChange;

namespace Appointly.Application.Services.RoleChange
{
    public interface IRoleChangeService
    {
        Task AddRoleChangeRequest(AddRoleChangeRequestDto request);
        Task<List<RoleChangeResponseDto>> GetAllRequests(RoleChangeRequestQueryDto query);
        Task DeleteRoleChangeRequest(Guid id);
    }
}
