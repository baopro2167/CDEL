using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ServiceSMRepo
{
    public interface IServiceSMRepository
    {
        Task<IEnumerable<ServiceSampleMethod>> GetAllAsync();
        Task<ServiceSampleMethod> GetByIdAsync(int id);
        IQueryable<ServiceSampleMethod> GetAll();
        Task<ServiceSampleMethod> AddAsync(ServiceSampleMethod serviceSampleMethod);
        Task<ServiceSampleMethod> UpdateAsync(ServiceSampleMethod serviceSM);
        Task DeleteAsync(int id);
    }
}
