using Appointly.Api.Common.DTOs.Admin;
using Appointly.Api.Common.DTOs.Advertisment;
using Appointly.Application.Services.Admin;
using Appointly.Domain.Common.Constants;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("admin")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsers();
            return Ok(_mapper.Map<List<UserResponseDto>>(users));
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
                return Ok("User Promoted to Admin");
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
            var advertisement = await _adminService.ApproveAdvertisment(advertisementId);
            return Ok(_mapper.Map<AdvertisementDetailResponseDto>(advertisement));
        }

        [HttpPost("advertisement/{advertisementId:guid}/reject")]
        public async Task<IActionResult> RejectAdvertisement([FromRoute] Guid advertisementId, [FromBody] RejectAdvertismentRequestDto requestDto)
        {
            var advertisement = await _adminService.RejectAdvertisment(advertisementId, requestDto.RejectReason);
            return Ok(_mapper.Map<AdvertisementDetailResponseDto>(advertisement));
        }
    }
}
