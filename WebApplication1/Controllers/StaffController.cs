using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.StaffSS;

namespace WebApplication1.Controllers
{
    [Route("api/Staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        public StaffController(IStaffService staffService)
            => _staffService = staffService;

        [HttpPost]
        //[Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateStaffRequestDTO dto)
        {
            try
            {
                var result = await _staffService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Manager,Admin,Staff")]
        public async Task<IActionResult> GetById(int id)
        {
            var staff = await _staffService.GetByIdAsync(id);
            if (staff == null) return NotFound();
            return Ok(staff);
        }
    
      /// <summary>
        /// Cập nhật thông tin Staff theo ID
        /// </summary>
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStaffDTO dto)
        {
            if (dto == null)
                return BadRequest("Staff update data is required.");

            try
            {
                var updated = await _staffService.UpdateStaffAsync(id, dto);
                if (updated == null)
                    return NotFound($"Staff with ID {id} not found.");

                return Ok("Update staff thành công");
            }
            catch (ArgumentException ex)
            {
                // ví dụ: FullName/Email không hợp lệ
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Xóa Staff theo ID
        /// </summary>
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Có thể lấy trước để trả 404 nếu không tồn tại
                var exists = await _staffService.GetByIdAsync(id);
                if (exists == null)
                    return NotFound($"Staff with ID {id} not found.");

                await _staffService.DeleteStaffAsync(id);
                return Ok("Xóa thành công");
            }
            catch (KeyNotFoundException ex)
            {
                // Hoặc nếu service ném KeyNotFoundException
                return NotFound(ex.Message);
            }
        }
    }


    }

