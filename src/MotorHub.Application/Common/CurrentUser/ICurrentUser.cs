namespace MotorHub.Application.Common.CurrentUser
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid Id { get; }
        string Email { get; }
        IReadOnlyList<string> Roles { get; }
    }
}
