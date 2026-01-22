using Appointly.Domain.Common.Enum;

namespace Appointly.Domain.Entities
{
    public class RoleChangeRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string RequestedRole { get; set; } = null!;
        public RoleRequestTypes Status { get; set; } = RoleRequestTypes.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public Guid? ReviewedByAdminId { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }

        // Navigation Property for User
        public User? User { get; set; }
        public User? ReviewedByAdmin { get; set; }
    }
}
