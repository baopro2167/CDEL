﻿using Model;
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
        Task<IEnumerable<ExaminationRequest>> GetAllAsync(IEnumerable<string> includedStatusIds);
        Task<ExaminationRequest?> GetByIdAsync(int id);
        Task<PaginatedList<ExaminationRequest>> GetAll(int pageNumber, int pageSize);
        Task<PaginatedList<ExaminationRequest>> GetByAccountId(int Userid, int pageNumber, int pageSize);
        Task<IEnumerable<ExRequestCustomerDTO>> GetExaminationRequests(int userId, int pageNumber, int pageSize);
        Task<ExaminationRequest> AddAsync(AddExRequestDTO addExRequestDto);
        Task<ExaminationRequest?> UpdateAsync(int id, UpdateExRequestDTO updateExRequestDto);
        Task DeleteAsync(int id);
        Task<ExRequestResponseDTO> AcceptAsync(int requestId);

        Task<ExRequestAssignResponseDTO> AssignStaffAsync(int requestId, AssignStaffRequestDTO dto);

        Task<ExaminationRequestStatusResponseDTO> UpdateStatusAsync(int requestId, UpdateExaminationRequestStatusDTO dto);
    }
}
