using Appointly.Api.Common.DTOs.RoleChangeRequest;
using Appointly.Application.Services.RoleChange;
using Appointly.Application.Services.RoleChange.Contracts;
using Appointly.Domain.Common.Constants;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RoleChangeController : ControllerBase
    {
        private readonly IRoleChangeService _roleChangeService;
        private readonly IMapper _mapper;
        public RoleChangeController(IRoleChangeService roleChangeService, IMapper mapper)
        {
            _roleChangeService = roleChangeService;
            _mapper = mapper;
        }

        [HttpPost("change")]
        public async Task<IActionResult> AddRoleChange([FromBody] RoleChangeRequestDto request)
        {
            await _roleChangeService.AddRoleChangeRequest(request.UserId, request.RequestedRole);
            return Ok("Role Changed requested Succesfully");
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAllRequests([FromQuery] RoleChangeRequestQueryDto queryDto)
        {
            var results = await _roleChangeService.GetAllRequests(_mapper.Map<RoleChangeRequestQuery>(queryDto));
            return Ok(_mapper.Map<List<RoleChangeResponseDto>>(results));
        }
    }
}
