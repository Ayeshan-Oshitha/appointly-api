using MotorHub.Application.DTOs.RoleChange;

namespace MotorHub.Application.Services.RoleChange
{
    public interface IRoleChangeService
    {
        Task AddRoleChangeRequest(AddRoleChangeRequestDto request);
        Task<List<RoleChangeResponseDto>> GetAllRequests(RoleChangeRequestQueryDto query);
        Task DeleteRoleChangeRequest(Guid id);
    }
}
