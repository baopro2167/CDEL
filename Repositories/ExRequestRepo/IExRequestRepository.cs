using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ExRequestRepo
{
    public interface IExRequestRepository
    {
        Task<IEnumerable<ExaminationRequest>> GetAsync();
        Task<ExaminationRequest> GetByIdAsync(int id);

        IQueryable<ExaminationRequest> GetByAccountId(int id);
        Task<ExaminationRequest> AddAsync(ExaminationRequest examinationRequest);
        Task<ExaminationRequest> UpdateAsync(ExaminationRequest examinationRequest);
        Task DeleteAsync(int id);
        IQueryable<ExaminationRequest> GetAll();
    }
}
