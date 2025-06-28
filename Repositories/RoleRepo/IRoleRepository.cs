using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoleRepo
{
   public  interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);

    }
}
