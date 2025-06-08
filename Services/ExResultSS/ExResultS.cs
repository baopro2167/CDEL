using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.ExResultRepo;
using Model;
using Services.DTO;
using Repositories.Pagging;
namespace Services.ExResultSS
{
    public class ExResultS : IExResultS
    {
        private readonly IExResultRepository _exResultRepository;

        public ExResultS(IExResultRepository exResultRepository)
        {
            _exResultRepository = exResultRepository;
        }
        public async Task<ExaminationResult?> GetByIdAsync(int id)
        {
            return await _exResultRepository.GetByIdAsync(id);
        }


        public async Task<PaginatedList<ExaminationResult>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<ExaminationResult> Result = _exResultRepository.GetAll().AsQueryable();
            return await PaginatedList<ExaminationResult>.CreateAsync(Result, pageNumber, pageSize);
        }

        public async Task<IEnumerable<ExaminationResult>> GetAllAsync()
        {
            return await _exResultRepository.GetAsync();
        }

        
        public async Task<ExaminationResult> AddAsync(AddExResultDTO addExResultDto)
        {

            if (addExResultDto == null)
            {
                throw new ArgumentNullException(nameof(addExResultDto), "ExResult data is required.");
            }

           
            ExaminationResult newExaminationResult = new ExaminationResult
            {
                RequestId = addExResultDto.RequestId,
                FileUrl = addExResultDto.FileUrl,
                ResultDate = addExResultDto.ResultDate,
                CreateAt = addExResultDto.CreateAt.Kind == DateTimeKind.Unspecified
         ? DateTime.SpecifyKind(addExResultDto.CreateAt, DateTimeKind.Utc)
         : addExResultDto.CreateAt.ToUniversalTime(),
               
            };
            await _exResultRepository.AddAsync(newExaminationResult);
            return newExaminationResult;
        }

      
        public async Task<ExaminationResult?> UpdateAsync(int id, UpdateExResultDTO updateExResultDto)
        {
            if (updateExResultDto == null)
            {
                throw new ArgumentNullException(nameof(updateExResultDto), "ExResult data is required.");
            }

            var UExResul = await _exResultRepository.GetByIdAsync(id);
            if (UExResul == null)
            {
                throw new KeyNotFoundException($"ExResult with ID {id} not found.");
            }
            UExResul.UpdateAt = updateExResultDto.UpdateAt.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(updateExResultDto.UpdateAt, DateTimeKind.Utc)
                : updateExResultDto.UpdateAt.ToUniversalTime();
            UExResul.FileUrl = updateExResultDto.FileUrl;
            UExResul.ResultDate = updateExResultDto.ResultDate;

            await _exResultRepository.UpdateAsync(UExResul);
            return UExResul;
        }
        public async Task DeleteAsync(int id)
        {
            var DExResul = await _exResultRepository.GetByIdAsync(id);
            if (DExResul == null)
            {
                throw new KeyNotFoundException($"ExResult with ID {id} not found.");
            }
            await _exResultRepository.DeleteAsync(id);
        }
    }
}
