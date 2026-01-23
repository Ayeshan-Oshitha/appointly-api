using System.ComponentModel.DataAnnotations;

namespace Appointly.Api.Common.DTOs.RoleChangeRequest
{
    public class RoleChangeRequestDto
    {
        public Guid UserId { get; set; }

        [Required]
        public required string RequestedRole { get; set; } 
    }
}
