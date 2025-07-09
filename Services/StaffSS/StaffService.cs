using Model;
using Repositories.StaffRepo;
using Repositories.UserRepo;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StaffSS
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUserRepository _userRepository;
        public StaffService(IStaffRepository staffRepository, IUserRepository userRepository)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateStaffResponseDto> CreateAsync(CreateStaffRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("FullName is required.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");

            var staff = new Staff
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserId = dto.UserId // Assuming UserId is part of CreateStaffRequestDTO
            };

            await _staffRepository.AddAsync(staff);

            return new CreateStaffResponseDto
            {
                Id = staff.Id, //
                FullName = staff.FullName,
                Email = staff.Email,
                UserId = staff.UserId // Assuming UserId is part of CreateStaffResponseDto

            };
        }

        public async Task<CreateStaffResponseDto?> GetByIdAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null) return null;
            return new CreateStaffResponseDto
            {
               Id = staff.Id,
                FullName = staff.FullName,
                Email = staff.Email,
                UserId = staff.UserId // Assuming UserId is part of CreateStaffResponseDto


            };
        }
        public async Task<Staff?> UpdateStaffAsync(int id, UpdateStaffDTO updateStaffDto)
        {
            if (updateStaffDto == null)
                throw new ArgumentNullException(nameof(updateStaffDto), "Staff data is required.");

            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
                throw new KeyNotFoundException($"Staff with ID {id} not found.");

            // Cập nhật các trường
            staff.FullName = updateStaffDto.FullName;
            staff.Email = updateStaffDto.Email
                ;

            // Nếu entity có trường UpdatedAt, gán thêm:
            // staff.UpdatedAt = DateTime.UtcNow;

            await _staffRepository.UpdateAsync(staff);
            return staff;
        }
        public async Task DeleteStaffAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
                throw new KeyNotFoundException($"Staff with ID {id} not found.");

            await _staffRepository.DeleteAsync(id);
        }
    }
}

