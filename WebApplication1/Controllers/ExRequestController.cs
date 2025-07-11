using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services.DTO;
using Services.ExRequestSS;
namespace WebApplication1.Controllers
{
    [Route("api/ExRequest")]
    [ApiController]
    public class ExRequestController : ControllerBase
    {
        private readonly IExRequestS _exRequestService;

        public ExRequestController(IExRequestS exRequestService)
        {
            _exRequestService = exRequestService;
        }
        /// <summary>
        /// Cập nhật một phần thông tin yêu cầu kiểm tra (ServiceId, SampleMethodId, AppointmentTime)
        /// </summary>

        [HttpPatch("partial/{id}")]
        public async Task<IActionResult> UpdatePartial(int id, [FromBody] UpdateExRequestPartialDTO dto)
        {
            if (id <= 0)
            {
                return BadRequest("ID phải là số nguyên dương.");
            }

            try
            {
                var updatedRequest = await _exRequestService.UpdatePartialAsync(id, dto);
                if (updatedRequest == null)
                {
                    return NotFound($"Không tìm thấy yêu cầu với ID {id}.");
                }
                return Ok(updatedRequest);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi khi cập nhật dữ liệu.");
            }
        }




        /// <summary>
        /// get các exrequest theo NotAccept và Accepted.
        /// </summary>
        [HttpGet("status")]
        //[Authorize(Roles = "4")]
        public async Task<IActionResult> GetAll()
        {
            // Nếu muốn override, client có thể truyền ?statuses=Accepted,Processing
             var dtos = await _exRequestService.GetStatusesWithNamesAsync();
            return Ok(dtos);
        }
        /// <summary>
        /// Staff cập nhật tiến trình xét nghiệm (SampleCollected → Processing → Completed)
        /// </summary>
        [HttpPatch("{requestId}/status")]
        [Authorize(Roles = "3")]

        public async Task<ActionResult<ExaminationRequestStatusResponseDTO>> UpdateStatus(
            int requestId,
            [FromBody] UpdateExaminationRequestStatusDTO dto)
        {
            if (dto == null)
                return BadRequest("Request body must contain status.");

            try
            {
                var result = await _exRequestService.UpdateStatusAsync(requestId, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (InvalidOperationException inv)
            {
                return BadRequest(inv.Message);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
        }
        /// <summary>
        /// Lấy tất cả các yêu cầu kiểm tra được gán cho một nhân viên theo StaffId
        /// </summary>
        /// <param name="staffId">ID của nhân viên</param>
        /// <returns>Danh sách các yêu cầu kiểm tra</returns>
        
        [HttpGet("staff/{staffId}")]
     
        public async Task<IActionResult> GetRequestsByStaffId(int staffId)
        {
            if (staffId <= 0)
            {
                return BadRequest("StaffId phải là số nguyên dương.");
            }

            try
            {
                var requests = await _exRequestService.GetRequestsByStaffIdAsync(staffId);
                if (!requests.Any())
                {
                    return NotFound($"Không tìm thấy yêu cầu nào cho StaffId {staffId}.");
                }
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi khi lấy dữ liệu.");
            }
        }










        /// <summary>
        /// Manager phân công staff cho request (không nhận trong vòng 30 phút)
        /// </summary>
        [HttpPatch("{requestId}/assign")]
       
        public async Task<ActionResult<ExRequestAssignResponseDTO>> Assign(
            int requestId,
            [FromBody] AssignStaffRequestDTO dto)
        {
            if (dto == null)
                return BadRequest("Request body must contain staffId.");

            // Kiểm tra tồn tại request
            var existing = await _exRequestService.GetByIdAsync(requestId);
            if (existing == null)
                return NotFound($"Request with ID {requestId} not found.");

            try
            {
                var result = await _exRequestService.AssignStaffAsync(requestId, dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Xung đột khung giờ 30 phút
                return BadRequest(ex.Message);
            }
        }





        /// <summary>
        /// Manager xác nhận yêu cầu
        /// </summary>
        [HttpPost("{requestId}/accept")]
        [Authorize(Roles = "4")]
        [ProducesResponseType(typeof(ExRequestResponseDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Accept(int requestId)
        {
            try
            {
                var result = await _exRequestService.AcceptAsync(requestId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Request {requestId} not found." });
            }
        }
    









        /// <summary>
        /// Lấy  ExRequest theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ExRequestS = await _exRequestService.GetByIdAsync(id);
            if (ExRequestS == null) return NotFound();
            return Ok(ExRequestS);
        }

        /// <summary>
        /// Manger Lấy toàn bộ ExRequest
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "4")]
        public async Task<ActionResult<IEnumerable<ExaminationRequest>>> GetFiltered(
    [FromQuery] string[] excludeStatuses)
        {
            var list = await _exRequestService.GetAllAsync(excludeStatuses);
            return Ok(list);
        }
        /// <summary>
        /// lấy danh sách đơn hàng theo AccountId
        /// </summary>
        /// <param name="Userid">AccountId</param>
        /// <param name="pageNumber">Số Trang</param>
        /// <param name="pageSize">Số Đơn hàng trong 1 trang</param>
        /// <returns></returns>
        [HttpGet("account/{Userid}")]
        public async Task<ActionResult<IEnumerable<ExaminationRequest>>> GetByAccountId(int Userid, int pageNumber = 1, int pageSize = 10)
        {
            var AExRequest = await _exRequestService.GetByAccountId(Userid, pageNumber, pageSize);
            return Ok(AExRequest);
        }

        /// <summary>
        /// Lấy danh sách ExRequest có phân trang
        /// </summary>

        [HttpGet("paged")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var ExRequestS = await _exRequestService.GetAll(pageNumber, pageSize);
            return Ok(ExRequestS);
        }
        /// <summary>
        /// lấy lịch sử booking exresquest theo user id có phân trang
        /// </summary>
        [HttpGet("customer")]
        public async Task<ActionResult<IEnumerable<ExRequestCustomerDTO>>> GetExaminationRequests([FromQuery] int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Gọi service để lấy danh sách các yêu cầu kiểm tra của khách hàng
            var examinationRequests = await _exRequestService.GetExaminationRequests(userId, pageNumber, pageSize);

            // Trả về dữ liệu dưới dạng JSON
            return Ok(examinationRequests);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddExRequestDTO addExRequestDTO)
        {
            if (addExRequestDTO == null)
            {
                return BadRequest("ExRequest data is required.");
            }

            try
            {
                var addExRequest = await _exRequestService.AddAsync(addExRequestDTO);
                return CreatedAtAction(nameof(GetById), new { id = addExRequest.Id }, addExRequest);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // For validation errors
            }
        }

        /// <summary>
        /// Update ExRequest theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateExRequestDTO updateExRequestDTO)
        {
            if (updateExRequestDTO == null)
            {
                return BadRequest("ExRequest update data is required.");
            }


            var updateExRequest = await _exRequestService.UpdateAsync(id, updateExRequestDTO);
            if (updateExRequest == null)
            {
                return NotFound($"ExRequest with ID {id} not found.");
            }

            return Ok("Update ExRequest thành công"); // 204 No Content, no body





        }
        /// <summary>
        /// Xóa ExRequest
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var DExRequest = await _exRequestService.GetByIdAsync(id);
                if (DExRequest == null)
                {
                    return NotFound($"ExRequest with ID {id} not found.");
                }

                await _exRequestService.DeleteAsync(id);
                return Ok("Xóa thành công"); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting ExRequest {id}: {ex.Message}");
                return NotFound(ex.Message);
            }

        }
    }
}
