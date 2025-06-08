using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repositories.KitRepo
{
    public interface IKitRepository
    {
        Task<IEnumerable<Kit>> GetkitAsync();
        Task<Kit> GetByIdAsync(int id);
        Task<Kit> AddAsync(Kit kit);
        Task<Kit> UpdateAsync(Kit kit);
        Task DeleteAsync(int id);
        IQueryable<Kit> GetAll();
       
    }
}
