using Appointly.Application.DTOs.RoleChange;
using Appointly.Application.Services.RoleChange;
using Appointly.Domain.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RoleChangeController : ControllerBase
    {
        private readonly IRoleChangeService _roleChangeService;
        public RoleChangeController(IRoleChangeService roleChangeService)
        {
            _roleChangeService = roleChangeService;
        }

        [HttpPost("change")]
        public async Task<IActionResult> AddRoleChange([FromBody] AddRoleChangeRequestDto request)
        {
            await _roleChangeService.AddRoleChangeRequest(request);
            return Ok("Role Changed requested Succesfully");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllRequests([FromQuery] RoleChangeRequestQueryDto query)
        {
            var results = await _roleChangeService.GetAllRequests(query);
            return Ok(results);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRoleChangeRequest([FromRoute] Guid id)
        {
            await _roleChangeService.DeleteRoleChangeRequest(id);
            return Ok("Role change request deleted successfully");
        }
    }
}
