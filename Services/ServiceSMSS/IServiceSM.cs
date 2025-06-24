using Model;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceSMSS
{
    public interface IServiceSM
    {
        Task<ServiceSampleMethod?> GetByIdAsync(int id);

        Task<IEnumerable<ServiceSampleMethod>> GetAllAsync();

        Task<PaginatedList<Service>> GetAll(int pageNumber, int pageSize);
        Task<ServiceSampleMethod> AddAsync(AddServiceBDTO addServiceBDto);
        Task<ServiceSampleMethod?> UpdateAsync(int id, UpdateServiceBDTO updateServiceBDto);
        Task DeleteAsync(int id);
    }
}
