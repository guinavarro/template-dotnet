using Microsoft.AspNetCore.Mvc;
using Template.Domain.Interfaces.Services;
using Template.Domain.Models.Command;

namespace Template.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("refresh-token/{refreshToken}")]
        public async Task<IActionResult> RefreshToken(string refreshToken, CancellationToken ct)
        {
            var result = await _authService.RefreshAccessTokenAsync(refreshToken, ct);
            return Ok(result);
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> Register([FromBody] AuthCommand command, CancellationToken ct)
        {
            var result = await _authService.RegisterUser(command, ct);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Auth([FromBody] AuthCommand command, CancellationToken ct)
        {
            var result = await _authService.Authenticate(command);
            return Ok(result);
        }
    }
}
