using Model;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ExRequestSS
{
    public interface IExRequestS
    {
        Task<IEnumerable<ExaminationRequest>> GetAllAsync();
        Task<ExaminationRequest?> GetByIdAsync(int id);
        Task<PaginatedList<ExaminationRequest>> GetAll(int pageNumber, int pageSize);
        Task<PaginatedList<ExaminationRequest>> GetByAccountId(int Userid, int pageNumber, int pageSize);
        Task<ExaminationRequest> AddAsync(AddExRequestDTO addExRequestDto);
        Task<ExaminationRequest?> UpdateAsync(int id, UpdateExRequestDTO updateExRequestDto);
        Task DeleteAsync(int id);
        


    }
}
