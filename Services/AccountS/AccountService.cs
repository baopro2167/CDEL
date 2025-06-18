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
namespace Services.AccountS
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IConfiguration _config;
        public AccountService(IUserRepository userRepository, IPasswordHasher<User> hasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _config = configuration;
        }
        private async Task<TokenResponseDTO> CreateTokenResponse(User? user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            // 1. Tạo access token
            var accessToken = CreateToken(user);

            // 2. Sinh và lưu refresh token qua _userRepository
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            // 3. Trả về DTO
            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RoleId = user.RoleId
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
                RoleId = 2
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
    }
}
