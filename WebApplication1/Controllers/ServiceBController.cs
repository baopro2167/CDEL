using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services.DTO;
using Services.ServiceSS;
namespace WebApplication1.Controllers
{
    [Route("api/ServiceB")]
    [ApiController]
    public class ServiceBController : ControllerBase
    {
        private readonly IServiceBB _serviceBB;
        public ServiceBController(IServiceBB serviceBB)
        {
            _serviceBB = serviceBB;
        }
        /// <summary>
        /// Lấy  serviceBB theo price
        /// </summary>
        [HttpGet("Price")]
        public async Task<IActionResult> GetPricing()
        {
            var pricing = await _serviceBB.GetPricingAsync();
            return Ok(pricing); // Trả về danh sách dịch vụ với thông tin bảng giá
        }


        /// <summary>
        /// Lấy  serviceBB theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sservice = await _serviceBB.GetByIdAsync(id);
            if (sservice == null) return NotFound();
            return Ok(sservice);
        }
        /// <summary>
        /// Lấy toàn bộ serviceBB
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetAll()
        {
            var sservice = await _serviceBB.GetAllAsync();
            return Ok(sservice);
        }
        /// <summary>
        /// Lấy danh sách serviceBB có phân trang
        /// </summary>

        [HttpGet("paged")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var sservice = await _serviceBB.GetAll(pageNumber, pageSize);
            return Ok(sservice);
        }
        /// <summary>
        /// Create serviceBB for STAFF
        /// </summary>

        [Authorize(Roles = "3")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddServiceBDTO addServiceBDTO)
        {
            if (addServiceBDTO == null)
            {
                return BadRequest("exResult data is required.");
            }

            try
            {
                // Truyền cả Service và SampleMethodIds vào phương thức AddAsync
                var addsserviceB = await _serviceBB.AddAsync(addServiceBDTO );

                return CreatedAtAction(nameof(GetById), new { id = addsserviceB.ServiceId }, addsserviceB);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Xử lý lỗi kiểm tra dữ liệu
            }
        }
        /// <summary>
        /// Update serviceBB theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateServiceBDTO updateServiceBDTO)
        {
            if (updateServiceBDTO == null)
            {
                return BadRequest("ServiceB update data is required.");
            }


            var updateServiceB = await _serviceBB.UpdateAsync(id, updateServiceBDTO);
            if (updateServiceB == null)
            {
                return NotFound($"ServiceB with ID {id} not found.");
            }

            return Ok("Update serviceBB thành công");






        }
        /// <summary>
        /// Xóa serviceBB
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var DServiceB = await _serviceBB.GetByIdAsync(id);
                if (DServiceB == null)
                {
                    return NotFound($"serviceBB with ID {id} not found.");
                }

                await _serviceBB.DeleteAsync(id);
                return Ok("Xóa thành công");  // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting serviceBB {id}: {ex.Message}");
                return NotFound(ex.Message);
            }

        }



    }
}
