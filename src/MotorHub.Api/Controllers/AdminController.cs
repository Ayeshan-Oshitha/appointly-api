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

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("PromoteToAdmin")]
        public async Task<IActionResult> PromoteToAdmin([FromQuery] Guid userId, Guid changeRoleRequestId)
        {
            var isAdmin = await _adminService.PromoteToAdmin(userId, changeRoleRequestId);
            if (isAdmin)
            {
                return Ok("User Promoted to Admin");
            }
            return BadRequest("User couldn't Promote to Admin");
        }

        [HttpPost("PromoteToSeller")]
        public async Task<IActionResult> PromoteToSeller([FromQuery] Guid userId, Guid changeRoleRequestId)
        {
            var isSeller = await _adminService.PromoteToSeller(userId, changeRoleRequestId);
            if (isSeller)
            {
                return Ok("User Promoted to Seller");
            }
            return BadRequest();
        }

        [HttpPost("RejectPromoteRequest")]
        public async Task<IActionResult> RejectPromoteRequest(
            [FromQuery] Guid userId,
            Guid changeRoleRequestId,
            [FromBody] RejectPromoteRequestDto requestDto)
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
        public async Task<IActionResult> RejectAdvertisement([FromRoute] Guid advertisementId, [FromBody] RejectAdvertisementRequestDto requestDto)
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
