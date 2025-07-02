using Model;
using Repositories.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using Repositories.Pagging;
using Repositories.RoleRepo;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Services.EmailS;
namespace Services.AccountS
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IConfiguration _config;
        private readonly IRoleRepository _roleRepository;
        private readonly string _googleClientId;
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailService _emailService;
        public AccountService(ILogger<AccountService> logger,IUserRepository userRepository, 
            IPasswordHasher<User> hasher, IConfiguration configuration, IRoleRepository roleRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _config = configuration;
            _roleRepository = roleRepository;
            _emailService = emailService;
            _logger = logger;
            _googleClientId = _config["GoogleAuth:ClientId"]!;
        }
        public async Task ForgotPasswordAsync(ForgotPasswordRequestDTO dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !user.Status)
                // Không tiết lộ user không tồn tại — chỉ kết thúc im lặng
                return;

            // 1) Sinh token ngẫu nhiên
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var token = Convert.ToBase64String(bytes);

            // 2) Lưu vào user với thời hạn 1 giờ
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateAsync(user);

            // 3) Gửi email cho user
            var resetLink = $"{_config["AppSettings:FrontendUrl"]}/reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";
            var html = $@"
            <p>Chào {user.Name},</p>
            <p>Bạn (hoặc ai đó) đã yêu cầu đặt lại mật khẩu. Vui lòng nhấn vào link dưới đây để reset mật khẩu (hết hạn sau 1h):</p>
            <p><a href=""{resetLink}"">Đặt lại mật khẩu</a></p>
            <p>Nếu bạn không yêu cầu, có thể bỏ qua email này.</p>
        ";
            var emailDto = new EmailDTO
            {
                To = user.Email,
                Subject = "Yêu cầu đặt lại mật khẩu",
                Body = html
            };
            await _emailService.SendAsync(emailDto);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDTO dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null
             || user.PasswordResetToken != dto.Token
             || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                throw new ApplicationException("Token không hợp lệ hoặc đã hết hạn.");
            }

            // Cập nhật mật khẩu mới
            user.Password = _hasher.HashPassword(user, dto.NewPassword);
            // Xóa token để tránh dùng lại
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _userRepository.UpdateAsync(user);
        }



        private async Task<TokenResponseDTO> CreateTokenResponse(User? user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            // 1. Tạo access token
            var accessToken = CreateToken(user);

            // 2. Sinh và lưu refresh token qua _userRepository
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            var role = await _roleRepository.GetByIdAsync(user.RoleId);

            // 3. Trả về DTO
            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RoleId = user.RoleId,
                RoleName = role?.Name ?? "Unknown Role"
            };
        }
        public async Task<User> Register(RegisterUserDto registerUserDto)
        {

            var existingUser = await _userRepository.GetByEmailAsync(registerUserDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                Name = registerUserDto.Name,
                Email = registerUserDto.Email,

                Phone = registerUserDto.Phone,
                Address = registerUserDto.Address,
                RoleId = 2,
                Status = true,
            };
            user.Password = _hasher.HashPassword(user, registerUserDto.Password);
            await _userRepository.Register(user);

            return user;
        }

        public async Task<TokenResponseDTO?> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginUserDto.Email);
            if (user is null)
                throw new ApplicationException("Email chưa được đăng ký.");

            // 2) Verify mật khẩu
            var result = _hasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new ApplicationException("Sai mật khẩu.");

            return await CreateTokenResponse(user);
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)),
            new Claim(ClaimTypes.Role, Convert.ToString(user.RoleId)),
            // thêm claim roles, email… nếu cần
        };

            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(_config.GetValue<String>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config.GetValue<String>("AppSettings:Issuer"),
                audience: _config.GetValue<String>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }




        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            // 1. Sinh refresh token mới
            var refreshToken = GenerateRefreshToken();

            // 2. Gán vào entity và set expiry
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            // 3. Lưu thay đổi vào database
            await _userRepository.UpdateAsync(user);

            // 4. Trả về token để gửi cho client
            return refreshToken;
        }
        public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken)
        {
            var user = await ValidateRefreshTokenAsync(refreshToken.UserId, refreshToken.RefreshToken);
            if (user is null)
                return null;

            // 2. Nếu hợp lệ, sinh lại access + refresh token mới
            return await CreateTokenResponse(user);
        }




        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            // 1. Lấy user từ repository (thay vì context.Users.FindAsync)
            var user = await _userRepository.GetByIdAsync(userId);

            // 2. Kiểm tra tồn tại, token khớp và chưa hết hạn
            if (user == null
                || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }
        public async Task<User> RegisterForAdmin(RegisterAdminDTO registerAdmin)
        {
            if (registerAdmin.RoleId == 1)
                throw new ApplicationException("Không được phép tạo user với roleId = 1.");
            var existingUser = await _userRepository.GetByEmailAsync(registerAdmin.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }
            var user = new User
            {

                Email = registerAdmin.Email,


                RoleId = registerAdmin.RoleId,
            };
            user.Password = _hasher.HashPassword(user, registerAdmin.Password);
            await _userRepository.Register(user);

            return user;
        }

        public async Task<PaginatedList<User>> GetByAccountRole(int roleid, int pageNumber, int pageSize)
        {
            IQueryable<User> requests = _userRepository.GetUsersByRoleId(roleid).AsQueryable();
            return await PaginatedList<User>.CreateAsync(requests, pageNumber, pageSize);
        }

        public async Task<IEnumerable<GetAllUserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAsync();
            var nowUtc = DateTime.UtcNow;

            // Chuyển từ User entity sang UserResponseDto
            var dtos = users.Select(u => new GetAllUserResponseDto
            {
                UserId = u.Id,
                Email = u.Email,
                Role = u.RoleId switch
                {
                    1 => "System Admin",
                    2 => "User",
                    3 => "Staff",
                    _ => "Unknown"
                },
                Status = u.Status
            });

            return dtos;
        }
        public async Task<GoogleLoginResponseDTO> LoginWithGoogleAsync(GoogleLoginRequestDTO dto)
        {
            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(
                   dto.IdToken,
                   new GoogleJsonWebSignature.ValidationSettings
                   {
                       Audience = new[] { _googleClientId }
                   });
            }
            catch (InvalidJwtException ex)
            {
                // hoặc: throw new ApplicationException($"Invalid Google token: {ex.Message}");
                _logger?.LogWarning(ex, "Google token invalid");
                throw new ApplicationException("Google token không hợp lệ.");
            }
            catch (Exception ex)
            {
                // Các lỗi khác
                _logger.LogError(ex, "Unexpected error validating Google token");
                throw;
            }

            // Nếu tới đây nghĩa là token hợp lệ
            var user = await _userRepository.GetByEmailAsync(payload.Email!);
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email!,
                    Name = payload.Name,
                    RoleId = 2,
                    Status = true
                };
                await _userRepository.Register(user);
            }

            var tokens = await CreateTokenResponse(user);
            return new GoogleLoginResponseDTO
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                RoleId = tokens.RoleId,
                RoleName = tokens.RoleName
            };

        }





    }
}
    

