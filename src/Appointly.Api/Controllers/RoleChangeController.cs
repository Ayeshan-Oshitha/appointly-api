using Appointly.Api.Common.DTOs.RoleChangeRequest;
using Appointly.Application.Services.RoleChange;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> AddRoleChange([FromBody] RoleChangeRequestDto request)
        {
            await _roleChangeService.AddRoleChangeRequest(request.UserId, request.RequestedRole);
            return Ok("Role Changed requested Succesfully");
        }


    }
}
