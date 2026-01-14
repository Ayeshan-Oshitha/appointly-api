using Appointly.Api.Common.DTOs.Admin;
using Appointly.Application.Services.Admin;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsers();
            return Ok(_mapper.Map<List<UserResponseDto>>(users));
        }
    }
}
