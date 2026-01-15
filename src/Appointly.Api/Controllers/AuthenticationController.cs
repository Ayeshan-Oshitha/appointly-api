using Appointly.Api.Common.DTOs.Admin;
using Appointly.Api.Common.DTOs.Authentication;
using Appointly.Application.Services.Authentication;
using Appointly.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Appointly.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto requestDto)
        {
            var registerResult = await _authenticationService.Register(
                requestDto.FirstName,
                requestDto.LastName,
                requestDto.Email,
                requestDto.Password,
                requestDto.PhoneNumber
                );

            var responseDto = _mapper.Map<RegisterResponseDto>(registerResult);
            return Ok(responseDto);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            var loginResult = await _authenticationService.Login(
                requestDto.Email,
                requestDto.Password);

            var responseDto = _mapper.Map<LoginResponseDto>(loginResult);
            return Ok(responseDto);
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _authenticationService.GetCurrentUserProfile();
            return Ok(_mapper.Map<UserResponseDto>(profile));
        }

    }
}
