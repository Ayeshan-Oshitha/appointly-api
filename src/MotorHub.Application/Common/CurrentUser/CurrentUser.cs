
using MotorHub.Domain.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MotorHub.Application.Common.CurrentUser
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public Guid Id => GetRequiredGuidClaim(ClaimTypes.NameIdentifier);

        public string Email => GetRequiredClaimValue(ClaimTypes.Email);

        public IReadOnlyList<string> Roles => GetUserRoles();

        private ClaimsPrincipal GetAuthenticatedUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            return user;
        }

        private string GetRequiredClaimValue(string claimType)
        {
            var user = GetAuthenticatedUser();
            var claim = user.FindFirst(claimType);
            
            if (claim == null || string.IsNullOrEmpty(claim.Value))
            {
                throw new UnauthorizedException("Required claim is missing.");
            }

            return claim.Value;
        }

        // A malformed subject claim is a bad token, so it belongs with the other 401s rather
        // than escaping as a FormatException and surfacing as a 500.
        private Guid GetRequiredGuidClaim(string claimType)
        {
            if (!Guid.TryParse(GetRequiredClaimValue(claimType), out var value))
            {
                throw new UnauthorizedException("Required claim is malformed.");
            }

            return value;
        }

        private IReadOnlyList<string> GetUserRoles()
        {
            var user = GetAuthenticatedUser();
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            return roles;
        }
    }
}
