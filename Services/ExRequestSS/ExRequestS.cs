﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.ExRequestRepo;
using Repositories.Pagging;
using Repositories.SampleMethodRepo;
using Repositories.ServiceRepo;
using Repositories.ServiceSMRepo;
using Repositories.StaffRepo;
using Repositories.UserRepo;
using Services.DTO;
using Services.EmailS;
namespace Services.ExRequestSS
{
    public class ExRequestS : IExRequestS

    {
        private readonly IExRequestRepository _exRequestRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IStaffRepository _staffRepo;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ISampleMethodRepository _sampleMethodRepository;

        private static readonly string[] DefaultStatuses = new[] { "1", "2" };
        public ExRequestS(IExRequestRepository exRequestRepository, IServiceRepository serviceRepository
            , IStaffRepository staffRepository, IUserRepository userRepository , ISampleMethodRepository sampleMethodRepository
            , IEmailService emailService)
        {
            _exRequestRepository = exRequestRepository;
            _serviceRepository = serviceRepository;
            _staffRepo = staffRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _sampleMethodRepository = sampleMethodRepository;
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
            var allowed = new[] { "3", "4", "5" };
            if (!allowed.Contains(dto.Status))
                throw new InvalidOperationException($"Status phải là một trong: {string.Join(", ", allowed)}.");

            // 3. (Tuỳ chọn) kiểm tra thứ tự chuyển trạng thái nếu cần
            string statusName = GetStatusName(dto.Status);

            // 4. Cập nhật
            req.StatusId = dto.Status;
            req.UpdateAt = DateTime.UtcNow;
            await _exRequestRepository.UpdateAsync(req);

            // 5. Build response
            return new ExaminationRequestStatusResponseDTO
            {
                RequestId = req.Id,
                Status = req.StatusId,
                StatusName = statusName,
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
        public async Task<PaginatedList<ExStatusResponeGetDTO>> GetAll(int pageNumber, int pageSize)
        {
            var query = _exRequestRepository.GetAll()
         .AsNoTracking()
         .OrderByDescending(x => x.Id);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();  // 🟢 Lấy toàn bộ về RAM trước

            var dtoList = data.Select(x => new ExStatusResponeGetDTO
            {
                Id = x.Id,
                StatusId = x.StatusId,
                StatusName = GetStatusName(x.StatusId),  // 🟢 An toàn ở C#
                UserId = x.UserId,
                ServiceId = x.ServiceId,
                SampleMethodId = x.SampleMethodId,
                AppointmentTime = x.AppointmentTime,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                StaffId = x.StaffId
            }).ToList();
            return new PaginatedList<ExStatusResponeGetDTO>(dtoList, totalCount, pageNumber, pageSize);
        }
        public async Task<PaginatedList<ExStatusResponeGetDTO>> GetByAccountId(int Userid, int pageNumber, int pageSize)
        {
            var query = _exRequestRepository.GetByAccountId(Userid)
        .AsNoTracking()
        .OrderByDescending(x => x.Id); // Thêm sort nếu cần

            // 2. Tổng số bản ghi (cho phân trang)
            var totalCount = await query.CountAsync();

            // 3. Lấy dữ liệu theo trang
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(); // ⚠️ Cần ToList trước khi gọi GetStatusName

            // 4. Mapping sang DTO sau khi lấy về
            var dtoList = data.Select(x => new ExStatusResponeGetDTO
            {
                Id = x.Id,
                StatusId = x.StatusId,
                StatusName = GetStatusName(x.StatusId),
                UserId = x.UserId,
                ServiceId = x.ServiceId,
                SampleMethodId = x.SampleMethodId,
                AppointmentTime = x.AppointmentTime,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                StaffId = x.StaffId
            }).ToList();

            // 5. Trả về kết quả phân trang
            return new PaginatedList<ExStatusResponeGetDTO>(dtoList, totalCount, pageNumber, pageSize);
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
                StatusName = GetStatusName(request.StatusId ?? "Unknown"),
                // Trả về StatusId kiểu Boolean

            });


            return examinationRequestDTOs;
        }



        public async Task<ExaminationResponeDTO> AddAsync(AddExRequestDTO addExRequestDto)
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
                StatusId = "1",
                AppointmentTime = addExRequestDto.AppointmentTime.Kind == DateTimeKind.Unspecified
         ? DateTime.SpecifyKind(addExRequestDto.AppointmentTime, DateTimeKind.Utc)
         : addExRequestDto.AppointmentTime.ToUniversalTime(),
            };
          
            await _exRequestRepository.AddAsync(examinationRequest);
            var user = await _userRepository.GetByIdAsync(addExRequestDto.UserId);
            var service = await _serviceRepository.GetByIdAsync(addExRequestDto.ServiceId);
            var sampleMethod = await _sampleMethodRepository.GetByIdAsync(addExRequestDto.SampleMethodId);
            var body = $@"
      <p>Xin chào {user.Name},</p>
      <p>Yêu cầu khám của bạn đã được ghi nhận:</p>
      <ul>
          <li><strong>Số điện thoại:</strong> {user.Phone}</li>
         <li><strong>Địa chỉ:</strong> {user.Address}</li>
        <li><strong>Dịch vụ:</strong> {service.Name}</li>
        <li><strong>Giá dịch vụ:</strong> {service.Price:#,##0}</li>
        <li><strong>Phương pháp lấy mẫu:</strong> {sampleMethod.Name}</li>
        <li><strong>Thời gian hẹn:</strong> {examinationRequest.AppointmentTime:yyyy-MM-dd HH:mm} UTC</li>
      </ul>";

            // 4. Gửi mail (giữ nguyên EmailService)
            await _emailService.SendAsync(new EmailDTO
            {
                To = user.Email,
                Subject = "Gửi yêu cầu khám thành công",
                Body = body
            });
            string statusName = GetStatusName(examinationRequest.StatusId);
            return new ExaminationResponeDTO
            {
                Id = examinationRequest.Id,
                UserId = examinationRequest.UserId,
                UserName = user.Name,
                ServiceId = examinationRequest.ServiceId,
                ServiceName = service.Name,
                ServicePrice = service.Price,
                SampleMethodId = examinationRequest.SampleMethodId,
                SampleMethodName = sampleMethod.Name,
                StatusId = examinationRequest.StatusId,
                StatusName = statusName,
                AppointmentTime = examinationRequest.AppointmentTime.ToUniversalTime(),
                CreateAt = examinationRequest.CreateAt.ToUniversalTime(),

                StaffId = examinationRequest.StaffId
            };
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
            req.StatusId = "2";
            req.UpdateAt = DateTime.UtcNow;
            await _exRequestRepository.UpdateAsync(req);
            string statusName = GetStatusName(req.StatusId);

            // 3. Trả về DTO
            return new ExRequestResponseDTO
            {
                RequestId = req.Id,
                Status = req.StatusId,
                StatusName = statusName,
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

        public async Task<IEnumerable<ExStatusRequestDTO>> GetStatusesWithNamesAsync(IEnumerable<string>? includedStatusIds= null)
        {
            var statuses = (includedStatusIds != null && includedStatusIds.Any())
                 
       ? includedStatusIds
       : DefaultStatuses;

            var requests = await _exRequestRepository.GetAll()
                .Include(r => r.User)           // Load User
        .Include(r => r.Service)        // Load Service
        .Include(r => r.SampleMethod)   // Load SampleMethod
          .Where(r => statuses.Contains(r.StatusId))
          .ToListAsync();

            var staffIds = requests.Select(r => r.StaffId).Distinct();
            var staffList = await _staffRepo.GetAllByIdsAsync(staffIds);
            var staffDict = staffList.ToDictionary(s => s.Id, s => s.FullName);

            return requests.Select(r => new ExStatusRequestDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User?.Name ,
                ServiceId = r.ServiceId,
                ServiceName = r.Service?.Name ,
                SampleMethodId = r.SampleMethodId,
                SampleMethodName = r.SampleMethod?.Name ,
                StatusId = r.StatusId,
                StatusName = GetStatusName(r.StatusId),
                AppointmentTime = r.AppointmentTime,
                CreateAt = r.CreateAt,
                UpdateAt = r.UpdateAt,
                StaffId = r.StaffId,
                StaffName = staffDict.ContainsKey(r.StaffId) ? staffDict[r.StaffId] : "Không rõ"
            });

        }
        public async Task<IEnumerable<ExStatusRequestDTO>> GetRequestsByStaffIdAsync(int staffId)
        {
            var requests = await _exRequestRepository.GetAll()
                .Include(r => r.User)
                .Include(r => r.Service)
                .Include(r => r.SampleMethod)
                .Where(r => r.StaffId == staffId)
                .ToListAsync();

            var staffIds = requests.Select(r => r.StaffId).Distinct();
            var staffList = await _staffRepo.GetAllByIdsAsync(staffIds);
            var staffDict = staffList.ToDictionary(s => s.Id, s => s.FullName);

            return requests.Select(r => new ExStatusRequestDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User?.Name ?? "Không rõ",
                ServiceId = r.ServiceId,
                ServiceName = r.Service?.Name ?? "Không rõ",
                SampleMethodId = r.SampleMethodId,
                SampleMethodName = r.SampleMethod?.Name ?? "Không rõ",
                StatusId = r.StatusId,

                StatusName = GetStatusName(r.StatusId ?? "Unknown"),
                AppointmentTime = r.AppointmentTime,
                CreateAt = r.CreateAt,
                UpdateAt = r.UpdateAt,
                StaffId = r.StaffId,
                StaffName = staffDict.ContainsKey(r.StaffId) ? staffDict[r.StaffId] : "Không rõ"
            });
        }
        public async Task<ExaminationRequest?> UpdatePartialAsync(int id, UpdateExRequestPartialDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Examination request data is required.");
            }

            var examinationRequest = await _exRequestRepository.GetByIdAsync(id);
            if (examinationRequest == null)
            {
                return null;
            }

            // Cập nhật các trường nếu có giá trị
            if (dto.ServiceId.HasValue)
                examinationRequest.ServiceId = dto.ServiceId.Value;
            if (dto.SampleMethodId.HasValue)
                examinationRequest.SampleMethodId = dto.SampleMethodId.Value;
            if (dto.AppointmentTime.HasValue)
                examinationRequest.AppointmentTime = dto.AppointmentTime.Value.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(dto.AppointmentTime.Value, DateTimeKind.Utc)
                    : dto.AppointmentTime.Value.ToUniversalTime();

            examinationRequest.UpdateAt = DateTime.UtcNow;
            await _exRequestRepository.UpdateAsync(examinationRequest);

            return examinationRequest;
        }









        private static  string GetStatusName(string statusId)
        {
            return statusId switch
            {
                "1" => "Not Accept",
                "2" => "Accepted",
                "3" => "SampleCollected",
                "4" => "Processing",
                "5" => "Completed",
                _ => "Unknown" // Trường hợp không xác định
            };
        }
    }
    }

