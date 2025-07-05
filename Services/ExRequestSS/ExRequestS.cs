using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.ExRequestRepo;
using Repositories.Pagging;
using Repositories.ServiceRepo;
using Repositories.ServiceSMRepo;
using Repositories.StaffRepo;
using Services.DTO;
namespace Services.ExRequestSS
{
    public class ExRequestS : IExRequestS

    {
        private readonly IExRequestRepository _exRequestRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IStaffRepository _staffRepo;
        public ExRequestS(IExRequestRepository exRequestRepository, IServiceRepository serviceRepository
            , IStaffRepository staffRepository)
        {
            _exRequestRepository = exRequestRepository;
            _serviceRepository = serviceRepository;
            _staffRepo = staffRepository;
        }
        public async Task<ExaminationRequestStatusResponseDTO> UpdateStatusAsync(int requestId, UpdateExaminationRequestStatusDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.", nameof(dto));

            // 1. Lấy request
            var req = await _exRequestRepository.GetByIdAsync(requestId);
            if (req == null)
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");

            // 2. Kiểm tra giá trị status hợp lệ
            var allowed = new[] { "SampleCollected", "Processing", "Completed" };
            if (!allowed.Contains(dto.Status))
                throw new InvalidOperationException($"Status phải là một trong: {string.Join(", ", allowed)}.");

            // 3. (Tuỳ chọn) kiểm tra thứ tự chuyển trạng thái nếu cần

            // 4. Cập nhật
            req.StatusId = dto.Status;
            req.UpdateAt = DateTime.UtcNow;
            await _exRequestRepository.UpdateAsync(req);

            // 5. Build response
            return new ExaminationRequestStatusResponseDTO
            {
                RequestId = req.Id,
                Status = req.StatusId,
                UpdatedAt = req.UpdateAt
            };
        }


        public async Task<IEnumerable<ExaminationRequest>> GetAllAsync(IEnumerable<string> includedStatusIds)
        {
            return await _exRequestRepository
         .GetAll()
         .Where(r => includedStatusIds.Contains(r.StatusId))
         .ToListAsync();
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


        public async Task<IEnumerable<ExRequestCustomerDTO>> GetExaminationRequests(int userId, int pageNumber, int pageSize)
        {
            // Lấy tất cả các yêu cầu kiểm tra của khách hàng
            IQueryable<ExaminationRequest> requests = _exRequestRepository.GetByAccountId(userId).AsQueryable();

            // Phân trang các yêu cầu kiểm tra
            var paginatedRequests = await PaginatedList<ExaminationRequest>.CreateAsync(requests, pageNumber, pageSize);

            // Chuyển đổi dữ liệu sang ExaminationRequestDTO
            var examinationRequestDTOs = paginatedRequests.Items.Select(request => new ExRequestCustomerDTO
            {
                RequestId = request.Id,
                ServiceName = _serviceRepository.GetByIdAsync(request.ServiceId).Result.Name,  // Lấy tên dịch vụ từ Service repository
                StatusId = request.StatusId,  
                // Trả về StatusId kiểu Boolean
               
            });


            return examinationRequestDTOs;
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
                StatusId = "Not Accept",
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
            examinationRequest.UpdateAt = DateTime.UtcNow;
            updateExRequestDto.AppointmentTime.ToUniversalTime();


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

        public async Task<ExRequestResponseDTO> AcceptAsync(int requestId)
        {
            // 1. Lấy request
            var req = await _exRequestRepository.GetByIdAsync(requestId);
            if (req == null)
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");

            // 2. Cập nhật trạng thái (giả sử statusId = 2 là Accepted)
            req.StatusId = "Accepted";
            req.UpdateAt = DateTime.UtcNow;
            await _exRequestRepository.UpdateAsync(req);

            // 3. Trả về DTO
            return new ExRequestResponseDTO
            {
                RequestId = req.Id,
                Status = req.StatusId,
                UpdatedAt = req.UpdateAt
            };
        }

        public async Task<ExRequestAssignResponseDTO> AssignStaffAsync(int requestId, AssignStaffRequestDTO dto)
        {
            // 0. Validate đầu vào cơ bản
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Request body must contain staffId.");

            // 1. Kiểm tra Staff tồn tại
            var staff = await _staffRepo.GetByIdAsync(dto.StaffId);
            if (staff == null)
                throw new KeyNotFoundException($"Staff with ID {dto.StaffId} not found.");

            // 2. Lấy request cần phân công
            var req = await _exRequestRepository.GetByIdAsync(requestId);
            if (req == null)
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");

            // 3. Kiểm tra xung đột trong khoảng 30 phút
            var lowerBound = req.AppointmentTime.AddMinutes(-30);
            var upperBound = req.AppointmentTime.AddMinutes(30);

            // 4. Kiểm tra conflict bằng BETWEEN
            bool hasConflict = await _exRequestRepository
                .GetAll()
                .Where(r => r.StaffId == dto.StaffId && r.Id != requestId)
                .AnyAsync(r =>
                    r.AppointmentTime >= lowerBound
                 && r.AppointmentTime <= upperBound
                );

            if (hasConflict)
                throw new InvalidOperationException("Staff đã được phân công vào khung giờ trong vòng 30 phút.");

            // 4. Gán Staff và lưu thay đổi
            req.StaffId = dto.StaffId;
            req.UpdateAt = DateTime.UtcNow;
            await _exRequestRepository.UpdateAsync(req);

            // 5. Build response
            return new ExRequestAssignResponseDTO
            {
                RequestId = req.Id,
                AssignedStaff = new AssignedStaffDTO
                {
                    StaffId = staff.Id,
                    StaffName = staff.FullName
                },
                UpdatedAt = req.UpdateAt
            };

        }

        
    }
    }

