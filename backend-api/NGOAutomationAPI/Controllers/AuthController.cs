using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NGOAutomationAPI.DTOs;
using NGOAutomationAPI.Services;

namespace NGOAutomationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _authService.Register(dto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _authService.Login(dto);
            if (result == "Invalid Credentials") return Unauthorized(new { message = result });

            return Ok(new { token = result });
        }
    }
}
