using Model;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SampleMethodSS
{
    public interface ISampleMethodS
    {
       
        Task<SampleMethod?> GetByIdAsync(int id);

        Task<IEnumerable<SampleMethod>> GetAllAsync();

        Task<PaginatedList<SampleMethod>> GetAll(int pageNumber, int pageSize);
        Task<SampleMethod> AddAsync(AddSampleMethodDTO addSampleMethodDto);
        Task<SampleMethod?> UpdateAsync(int id, UpdateSampleMethodDTO updateSampleMethodDto);
        Task DeleteAsync(int id);
       
    }
}
