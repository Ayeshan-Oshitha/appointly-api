using Appointly.Domain.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace Appointly.Api.Common.DTOs.RoleChangeRequest
{
    public class RoleChangeRequestQueryDto
    {
        public string? Search { get; set; }

        [EnumDataType(typeof(RoleRequestTypes))]
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
