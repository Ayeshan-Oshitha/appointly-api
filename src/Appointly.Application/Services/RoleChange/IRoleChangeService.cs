namespace Appointly.Application.Services.RoleChange
{
    public interface IRoleChangeService
    {
        Task AddRoleChangeRequest(Guid userId, string newRole);
    }
}
