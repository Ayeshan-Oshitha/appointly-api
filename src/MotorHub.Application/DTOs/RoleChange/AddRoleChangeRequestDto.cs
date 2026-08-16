using System.ComponentModel.DataAnnotations;

namespace MotorHub.Application.DTOs.RoleChange
{
    public class AddRoleChangeRequestDto
    {
        // The requesting user is always taken from the authenticated caller, never from the body.
        [Required]
        public required string RequestedRole { get; set; }
    }
}
