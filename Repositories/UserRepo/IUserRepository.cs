using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> Register(User user);

        Task UpdateAsync(User user);

        Task<User?> GetByIdAsync(int id);

        IQueryable<User> GetAll();
        IQueryable<User> GetUsersByRoleId(int roleId);

        Task<IEnumerable<User>> GetAsync();
    }
}
