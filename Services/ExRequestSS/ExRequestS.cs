using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repositories.ExRequestRepo;
using Repositories.Pagging;
using Services.DTO;
namespace Services.ExRequestSS
{
    public class ExRequestS : IExRequestS

    {
        private readonly IExRequestRepository _exRequestRepository;
        public ExRequestS(IExRequestRepository exRequestRepository)
        {
            _exRequestRepository = exRequestRepository;
        }
        public async Task<IEnumerable<ExaminationRequest>> GetAllAsync()
        {
            return await _exRequestRepository.GetAsync();
        }
        public async Task<ExaminationRequest?> GetByIdAsync(int id)
        {
            return await _exRequestRepository.GetByIdAsync(id);
        }
        public async Task<PaginatedList<ExaminationRequest>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<ExaminationRequest> requests = _exRequestRepository.GetAll().AsQueryable();
            return await PaginatedList<ExaminationRequest>.CreateAsync(requests, pageNumber, pageSize);
        }
        public async Task<PaginatedList<ExaminationRequest>> GetByAccountId(int Userid, int pageNumber, int pageSize)
        {
            IQueryable<ExaminationRequest> requests = _exRequestRepository.GetByAccountId(Userid).AsQueryable();
            return await PaginatedList<ExaminationRequest>.CreateAsync(requests, pageNumber, pageSize);
        }
        public async Task<ExaminationRequest> AddAsync(AddExRequestDTO addExRequestDto)
        {
            if (addExRequestDto == null)
            {
                throw new ArgumentNullException(nameof(addExRequestDto), "Examination request data is required.");
            }
            var examinationRequest = new ExaminationRequest
            {
                UserId = addExRequestDto.UserId,
                ServiceId = addExRequestDto.ServiceId,
                PriorityId = addExRequestDto.PriorityId,
                SampleMethodId = addExRequestDto.SampleMethodId,
                StatusId = addExRequestDto.StatusId,
                AppointmentTime = addExRequestDto.AppointmentTime.Kind == DateTimeKind.Unspecified
         ? DateTime.SpecifyKind(addExRequestDto.AppointmentTime, DateTimeKind.Utc)
         : addExRequestDto.AppointmentTime.ToUniversalTime(),
            };
            await _exRequestRepository.AddAsync(examinationRequest);
            return examinationRequest;
        }
        public async Task<ExaminationRequest?> UpdateAsync(int id, UpdateExRequestDTO updateExRequestDto)
        {
            if (updateExRequestDto == null)
            {
                throw new ArgumentNullException(nameof(updateExRequestDto), "Examination request data is required.");
            }
            var examinationRequest = await _exRequestRepository.GetByIdAsync(id);
            if (examinationRequest == null)
            {
                return null;
            }

            examinationRequest.ServiceId = updateExRequestDto.ServiceId;
            examinationRequest.PriorityId = updateExRequestDto.PriorityId;
            examinationRequest.SampleMethodId = updateExRequestDto.SampleMethodId;
            examinationRequest.StatusId = updateExRequestDto.StatusId;
            examinationRequest.AppointmentTime = updateExRequestDto.AppointmentTime.Kind == DateTimeKind.Unspecified
         ? DateTime.SpecifyKind(updateExRequestDto.AppointmentTime, DateTimeKind.Utc)
         : updateExRequestDto.AppointmentTime.ToUniversalTime();

            await _exRequestRepository.UpdateAsync(examinationRequest);
            return examinationRequest;
        }
        public async Task DeleteAsync(int id)
        {
            var examinationRequest = await _exRequestRepository.GetByIdAsync(id);
            if (examinationRequest == null)
            {
                throw new KeyNotFoundException($"Examination request with ID {id} not found.");
            }
            await _exRequestRepository.DeleteAsync(id);
        }
    }
}
