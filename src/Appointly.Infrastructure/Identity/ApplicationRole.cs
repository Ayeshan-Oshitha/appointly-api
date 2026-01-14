using Microsoft.AspNetCore.Identity;

namespace Appointly.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
