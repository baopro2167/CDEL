using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ExResultRepo
{
    public interface IExResultRepository
    {
        Task<IEnumerable<ExaminationResult>> GetAsync();
        Task<ExaminationResult> GetByIdAsync(int id);
        Task<ExaminationResult> AddAsync(ExaminationResult result);
        Task<ExaminationResult> UpdateAsync(ExaminationResult result);
        Task DeleteAsync(int id);
        IQueryable<ExaminationResult> GetAll();
    }
}
