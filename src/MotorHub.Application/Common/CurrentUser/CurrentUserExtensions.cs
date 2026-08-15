using MotorHub.Domain.Common.Constants;

namespace MotorHub.Application.Common.CurrentUser
{
    public static class CurrentUserExtensions
    {
        public static bool IsAdmin(this ICurrentUser currentUser)
        {
            return currentUser.Roles.Contains(Roles.Admin) || currentUser.Roles.Contains(Roles.SuperAdmin);
        }
    }
}
