using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SampleMethodRepo
{
    public interface ISampleMethodRepository
    {
        Task<IEnumerable<SampleMethod>> GetAsync();
        Task<SampleMethod> GetByIdAsync(int id);
        Task<SampleMethod> AddAsync(SampleMethod method);
        Task<SampleMethod> UpdateAsync(SampleMethod method);
        Task DeleteAsync(int id);
        IQueryable<SampleMethod> GetAll();
    }
}
