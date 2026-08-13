namespace Appointly.Application.DTOs.RoleChange
{
    public class RoleChangeResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? RequestedRole { get; set; }
        public string? Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public Guid? ReviewedByAdminId { get; set; }
        public string? ReviewedByAdminFullName { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }
    }
}
