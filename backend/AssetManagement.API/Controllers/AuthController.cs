
using Microsoft.AspNetCore.Mvc;
using AssetManagement.Core.DTOs;
using AssetManagement.Infrastructure.Services;


namespace AssetManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
            {
                return Unauthorized(new { message = "invalid usrname or password" });
            }
            return Ok(result);
        }
        


    }
}