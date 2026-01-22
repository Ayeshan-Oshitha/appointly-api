using Appointly.Domain.Common.Enum;

namespace Appointly.Application.Services.RoleChange.Contracts
{
    public class RoleChangeRequestQuery
    {
        public string? Search { get; set; }
        public RoleRequestTypes? RequestType { get; set; }
        public string? RoleType { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ReviewByAdminId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public RoleChangeSortBy? SortBy { get; set; }
        public RoleChangeSortOrder? SortOrder { get; set; }
    }
}
