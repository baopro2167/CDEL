using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoleRepo
{
    public class RoleRepository : IRoleRepository

    {
        private readonly BloodlineDbContext _context;

        public RoleRepository(BloodlineDbContext context)
        {
            _context = context;
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }
    }
}
