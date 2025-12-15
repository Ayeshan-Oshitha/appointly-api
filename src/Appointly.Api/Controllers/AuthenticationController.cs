using Appointly.Api.Common.DTOs.Authentication;
using Appointly.Application.Services.Authentication;
using Appointly.Application.Services.Authentication.DTOs;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Register(RegisterRequestDto requestDto)
        {
            var registerResult = _authenticationService.Register(
                requestDto.FirstName,
                requestDto.LastName,
                requestDto.Email,
                requestDto.Password);

            var responseDto = _mapper.Map<RegisterResponseDto>(registerResult);
            return Ok(responseDto);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto requestDto)
        {
            var loginResult = _authenticationService.Login(
                requestDto.Email,
                requestDto.Password);

            var responseDto = _mapper.Map<LoginResponseDto>(loginResult);
            return Ok(responseDto);
        }

    }
}
