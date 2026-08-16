using MotorHub.Application.DTOs.Authentication;
using MotorHub.Application.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MotorHub.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var registerResult = await _authenticationService.Register(request);
            return Ok(registerResult);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var loginResult = await _authenticationService.Login(request);
            return Ok(loginResult);
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _authenticationService.GetCurrentUserProfile();
            return Ok(profile);
        }

    }
}
