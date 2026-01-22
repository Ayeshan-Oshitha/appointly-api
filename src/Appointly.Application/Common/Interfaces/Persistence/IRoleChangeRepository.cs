namespace Appointly.Application.Common.Interfaces.Persistence
{
    public interface IRoleChangeRepository
    {
        Task AddRoleChangeRequestAsync(Guid userId, string newRole);
    }
}
