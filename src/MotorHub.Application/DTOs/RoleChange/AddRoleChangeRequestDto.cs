using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.RoleChange
{
    public class AddRoleChangeRequestDto
    {
        public Guid UserId { get; set; }

        [Required]
        public required string RequestedRole { get; set; }
    }
}
