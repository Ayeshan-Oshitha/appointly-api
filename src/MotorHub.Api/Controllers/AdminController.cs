using MotorHub.Application.DTOs.Admin;
using MotorHub.Application.Services.Admin;
using MotorHub.Domain.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MotorHub.Api.Controllers
{
    [Route("admin")]
    [ApiController]
    [Authorize(Policy = Policies.AdminOnly)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsers();
            return Ok(users);
        }

        // The role-request routes mirror the advertisement review routes below: the subject's id
        // in the path, the transition as the trailing verb. userId stays a query parameter because
        // AdminService cross-checks it against the request's own UserId.
        [HttpPost("role-requests/{changeRoleRequestId:guid}/promote-admin")]
        public async Task<IActionResult> PromoteToAdmin(
            [FromRoute] Guid changeRoleRequestId,
            [FromQuery] Guid userId)
        {
            var isAdmin = await _adminService.PromoteToAdmin(userId, changeRoleRequestId);
            if (isAdmin)
            {
                return Ok("User Promoted to Admin");
            }
            return BadRequest("User couldn't Promote to Admin");
        }

        [HttpPost("role-requests/{changeRoleRequestId:guid}/promote-seller")]
        public async Task<IActionResult> PromoteToSeller(
            [FromRoute] Guid changeRoleRequestId,
            [FromQuery] Guid userId)
        {
            var isSeller = await _adminService.PromoteToSeller(userId, changeRoleRequestId);
            if (isSeller)
            {
                return Ok("User Promoted to Seller");
            }
            return BadRequest("User couldn't Promote to Seller");
        }

        [HttpPost("role-requests/{changeRoleRequestId:guid}/reject")]
        public async Task<IActionResult> RejectPromoteRequest(
            [FromRoute] Guid changeRoleRequestId,
            [FromQuery] Guid userId,
            [FromBody] RejectReasonDto requestDto)
        {
            var isRejected = await _adminService.RejectPromoteRequest(userId, changeRoleRequestId, requestDto.RejectReason);
            if (isRejected)
            {
                return Ok("Role change request rejected successfully.");
            }
            return BadRequest("Failed to reject the role change request.");
        }

        [HttpPost("advertisement/{advertisementId:guid}/approve")]
        public async Task<IActionResult> ApproveAdvertisement([FromRoute] Guid advertisementId)
        {
            var advertisement = await _adminService.ApproveAdvertisement(advertisementId);
            return Ok(advertisement);
        }

        [HttpPost("advertisement/{advertisementId:guid}/reject")]
        public async Task<IActionResult> RejectAdvertisement([FromRoute] Guid advertisementId, [FromBody] RejectReasonDto requestDto)
        {
            var advertisement = await _adminService.RejectAdvertisement(advertisementId, requestDto.RejectReason);
            return Ok(advertisement);
        }

        [HttpPost("advertisement/{advertisementId:guid}/undo")]
        public async Task<IActionResult> UndoAdvertisementReview([FromRoute] Guid advertisementId)
        {
            var advertisement = await _adminService.UndoAdvertisementReview(advertisementId);
            return Ok(advertisement);
        }
    }
}
