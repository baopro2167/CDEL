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
        Task<ServiceGetDTO?> GetByIdAsync(int id);

        Task<IEnumerable<Service>> GetAllAsync();
        Task<IEnumerable<ServiceGetPriceDTO>> GetPricingAsync();
        Task<PaginatedList<Service>> GetAll(int pageNumber, int pageSize);
        Task<ServiceGetNameDTO> AddAsync(AddServiceBDTO addServiceBDto);
        Task<ServiceGetNameDTO?> UpdateAsync(int id, UpdateServiceBDTO updateServiceBDto);
        Task DeleteAsync(int id);

       

    }
}
