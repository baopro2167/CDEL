using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AccountS;
using Services.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (registerUserDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var user = await _accountService.Register(registerUserDto);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login([FromBody] LoginUserDto dto)
        {
            try
            {
                var result = await _accountService.LoginAsync(dto);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndPoint()
        {
            return Ok("dang nhap thanh cong");
        }
        [Authorize(Roles = "1")]
        [HttpGet("admin-only")]
        public IActionResult AuthenticatedADOnlyEndPoint()
        {
            return Ok("dang nhap admin thanh cong");
        }



        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO request)
        {
            var result = await _accountService.RefreshTokenAsync(request);

            if (result == null
                || result.AccessToken == null
                || result.RefreshToken == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(result);
        }
    }
}
