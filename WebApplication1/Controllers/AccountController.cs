using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
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
        /// <summary>
        /// Lấy thông tin user theo ID
        /// </summary>
        [HttpGet("{userId}")]
        
        public async Task<ActionResult<GetUserByIdResponseDTO>> GetById(int userId)
        {
            var dto = await _accountService.GetByIdAsync(userId);
            if (dto == null)
                return NotFound($"User with ID {userId} not found.");

            return Ok(dto);
        }



        /// <summary>
        /// Cập nhật profile (Name, Phone, Address)
        /// </summary>
        [HttpPatch("{userId}/profile")]
      
       
        public async Task<ActionResult<UpdateUserProfileResponseDTO>> UpdateProfile(
            int userId,
            [FromBody] UpdateUserProfileDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _accountService.UpdateProfileAsync(userId, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
        }






        /// <summary>
        /// Admin vô hiệu hóa user (status = false)
        /// </summary>
        [HttpPatch("{userId}/status")]
        [Authorize(Roles = "1")]
        
        public async Task<ActionResult<UpdateStatusResponseDTO>> Deactivate(int userId)
        {
            try
            {
                var result = await _accountService.DeactivateAsync(userId);
                return Ok(result);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
        }

        /// <summary>
        /// Lấy toàn bộ accountall
        /// </summary>
        [HttpGet("accountall")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var guser = await _accountService.GetAllAsync();
            return Ok(guser);
        }




        /// <summary>
        /// lấy danh sách account theo roleid
        /// </summary>
        /// <param name="roleid">roleid</param>
        /// <param name="pageNumber">Số Trang</param>
        /// <param name="pageSize">Số Đơn hàng trong 1 trang</param>
        /// <returns></returns>
        [HttpGet("account/{roleid}")]
        public async Task<ActionResult<IEnumerable<User>>> GetActionResultAsync(int roleid, int pageNumber = 1, int pageSize = 10)
        {
            var AExRequest = await _accountService.GetByAccountRole(roleid, pageNumber, pageSize);
            return Ok(AExRequest);
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
        [Authorize(Roles = "1")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDTO registerAdmin)
        {
            if (registerAdmin == null)
            {
                return BadRequest("Invalid create user data.");
            }
            try
            {
                var user = await _accountService.RegisterForAdmin(registerAdmin);
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


        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO dto)
        {
            try
            {
                var result = await _accountService.LoginWithGoogleAsync(dto);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                // lỗi token invalid/expired hoặc user không hợp lệ
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // lỗi không mong muốn
                // Bạn có thể log ex.InnerException ở đây nếu cần
                return StatusCode(500, new { message = "Có lỗi trong quá trình xử lý." });
            }
        }


        /// <summary>
        /// Gửi email quên mật khẩu
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO dto)
        {
            await _accountService.ForgotPasswordAsync(dto);
            // Trả 200 dù user tồn tại hay không, để không lộ thông tin
            return Ok(new { message = "Nếu email tồn tại, bạn sẽ nhận được hướng dẫn trong inbox." });
        }

        /// <summary>
        /// Đặt lại mật khẩu
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO dto)
        {
            try
            {
                await _accountService.ResetPasswordAsync(dto);
                return Ok(new { message = "Đặt lại mật khẩu thành công." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
