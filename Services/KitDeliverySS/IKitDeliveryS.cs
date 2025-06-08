using Model;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.KitDeliverySS
{
    public interface IKitDeliveryS
    {
        Task<IEnumerable<KitDelivery>> GetAllKitAsync();
        Task<KitDelivery?> GetByIdAsync(int id);
        Task<KitDelivery> AddAsync(AddKitDeliveryDTO addKitDeliveryDto);
        Task<KitDelivery?> UpdateAsync(int id, UpdateKitDeliveryDTO updateKitDeliveryDto);
        Task DeleteAsync(int id);
        Task<PaginatedList<KitDelivery>> GetAll(int pageNumber, int pageSize);


    }
}
