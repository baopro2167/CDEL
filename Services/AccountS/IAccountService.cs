using Model;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AccountS
{
    public interface IAccountService
    {
        Task<User> Register(RegisterUserDto registerUserDto);

        Task<TokenResponseDTO?> LoginAsync(LoginUserDto loginUserDto);

        Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken);
    }
}
