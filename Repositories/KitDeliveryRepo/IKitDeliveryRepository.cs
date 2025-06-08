using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.KitDeliveryRepo
{
    public interface IKitDeliveryRepository
    {
        Task<IEnumerable<KitDelivery>> GetkitAsync();
        Task<KitDelivery> GetByIdAsync(int id);
        Task<KitDelivery> AddAsync(KitDelivery kitDelivery);
        Task<KitDelivery> UpdateAsync(KitDelivery kitDelivery);
        Task DeleteAsync(int id);
        IQueryable<KitDelivery> GetAll();


    }
}
