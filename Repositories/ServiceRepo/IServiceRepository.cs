using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ServiceRepo
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<IEnumerable<Service>> GetPricingAsync();
        Task<Service> GetByIdAsync(int id);
        IQueryable<Service> GetAll();
        Task<Service> AddAsync(Service service);
        Task<Service> UpdateAsync(Service service);
        Task DeleteAsync(int id);
      
    }
}
