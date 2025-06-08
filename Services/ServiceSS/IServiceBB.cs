using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repositories.Pagging;
using Services.DTO;
namespace Services.ServiceSS
{
    public interface IServiceBB
    {
        Task<Service?> GetByIdAsync(int id);

        Task<IEnumerable<Service>> GetAllAsync();

        Task<PaginatedList<Service>> GetAll(int pageNumber, int pageSize);
        Task<Service> AddAsync(AddServiceBDTO addServiceBDto);
        Task<Service?> UpdateAsync(int id, UpdateServiceBDTO updateServiceBDto);
        Task DeleteAsync(int id);

    }
}
