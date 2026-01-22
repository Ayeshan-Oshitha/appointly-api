using Appointly.Api.Common.DTOs.Admin;
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
        public async Task<IActionResult> PromoteToAdmin([FromQuery] Guid userId)
        {
            var isAdmin = await _adminService.PromoteToAdmin(userId);
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
    }
}
