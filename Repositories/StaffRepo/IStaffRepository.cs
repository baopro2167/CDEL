using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StaffRepo
{
    public interface IStaffRepository
    {
        Task AddAsync(Staff staff);
        Task<Staff?> GetByIdAsync(int id);
        Task<IEnumerable<Staff>> GetAllAsync();

        Task<Staff> UpdateAsync(Staff staff);
        Task DeleteAsync(int id);

        Task<IEnumerable<Staff>> GetAllByIdsAsync(IEnumerable<int> ids);
    }
}
