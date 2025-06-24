using Model;
using Repositories.Pagging;
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
        Task<User> RegisterForAdmin(RegisterAdminDTO registerAdmin);
        Task<TokenResponseDTO?> LoginAsync(LoginUserDto loginUserDto);

        Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken);
        Task<PaginatedList<User>> GetByAccountRole(int roleid, int pageNumber, int pageSize);
        Task<IEnumerable<GetAllUserResponseDto>> GetAllAsync();
    }
}
