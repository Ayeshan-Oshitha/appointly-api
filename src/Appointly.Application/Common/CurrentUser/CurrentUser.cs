
using Appointly.Domain.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Appointly.Application.Common.CurrentUser
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id => Guid.Parse(GetRequiredClaimValue(ClaimTypes.NameIdentifier));

        public string Email => GetRequiredClaimValue(ClaimTypes.Email);

        private ClaimsPrincipal GetAuthenticatedUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
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
    }
}
