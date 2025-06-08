using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Services.DTO;
using Repositories.Pagging;
namespace Services.KitS
{
   public interface IKitService
    {
        Task<IEnumerable<Kit>> GetAllKitAsync();
        Task<Kit?> GetByIdAsync(int id);
        Task<Kit> AddKitAsync(AddKitDTO createKitDto);
        Task<Kit?> UpdateKitAsync(int id, UpdateKitDTO updateKitDto);
        Task DeleteKitAsync(int id);
        Task<PaginatedList<Kit>> GetAll(int pageNumber, int pageSize);
    }
}
