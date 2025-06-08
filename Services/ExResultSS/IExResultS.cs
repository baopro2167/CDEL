using Model;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ExResultSS
{
    public interface IExResultS
    {
        Task<IEnumerable<ExaminationResult>> GetAllAsync();
        Task<ExaminationResult?> GetByIdAsync(int id);
        Task<ExaminationResult> AddAsync(AddExResultDTO addExResultDto);
        Task<ExaminationResult?> UpdateAsync(int id, UpdateExResultDTO updateExResultDto);
        Task DeleteAsync(int id);
        Task<PaginatedList<ExaminationResult>> GetAll(int pageNumber, int pageSize);

    }
}
