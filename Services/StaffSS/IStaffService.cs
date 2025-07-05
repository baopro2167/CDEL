using Model;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StaffSS
{
    public interface IStaffService
    {
        Task<CreateStaffResponseDto> CreateAsync(CreateStaffRequestDTO dto);
        Task<CreateStaffResponseDto?> GetByIdAsync(int id);
        Task<Staff?> UpdateStaffAsync(int id, UpdateStaffDTO updateStaffDto);
        Task DeleteStaffAsync(int id);
    }
}
