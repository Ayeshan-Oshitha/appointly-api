namespace Appointly.Application.Common.CurrentUser
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        string Email { get; }
        IReadOnlyList<string> Roles { get; }
    }
}
