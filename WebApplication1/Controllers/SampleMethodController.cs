using Microsoft.AspNetCore.Mvc;
using Model;
using Services.DTO;
using Services.SampleMethodSS;
namespace WebApplication1.Controllers
{
    [Route("api/SampleMethod")]
    [ApiController]
    public class SampleMethodController : ControllerBase
    {
        private readonly ISampleMethodS _sampleMethodService;
        public SampleMethodController(ISampleMethodS sampleMethodService)
        {
            _sampleMethodService = sampleMethodService;
        }

        /// <summary>
        /// Lấy  sampleMethod theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sMethod = await _sampleMethodService.GetByIdAsync(id);
            if (sMethod == null) return NotFound();
            return Ok(sMethod);
        }

        /// <summary>
        /// Lấy toàn bộ sampleMethod
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SampleMethod>>> GetAll()
        {
            var sMethod = await _sampleMethodService.GetAllAsync();
            return Ok(sMethod);
        }
        /// <summary>
        /// Lấy danh sách sampleMethod có phân trang
        /// </summary>

        [HttpGet("paged")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var sMethod = await _sampleMethodService.GetAll(pageNumber, pageSize);
            return Ok(sMethod);
        }
        /// <summary>
        /// Create sampleMethod
        /// </summary>


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddSampleMethodDTO addsampleMethodDTO)
        {
            if (addsampleMethodDTO == null)
            {
                return BadRequest("exResult data is required.");
            }

            try
            {
                var addsampleMethod = await _sampleMethodService.AddAsync(addsampleMethodDTO);
                return CreatedAtAction(nameof(GetById), new { id = addsampleMethod.Id }, addsampleMethod);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // For validation errors
            }
        }

        /// <summary>
        /// Update sampleMethod theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateSampleMethodDTO updateSampleMethodDTO)
        {
            if (updateSampleMethodDTO == null)
            {
                return BadRequest("sampleMethod update data is required.");
            }


            var updateSampleMethod = await _sampleMethodService.UpdateAsync(id, updateSampleMethodDTO);
            if (updateSampleMethod == null)
            {
                return NotFound($"sampleMethod with ID {id} not found.");
            }

            return Ok("Update sampleMethod thành công");





        }

        /// <summary>
        /// Xóa sampleMethod
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var DMethod = await _sampleMethodService.GetByIdAsync(id);
                if (DMethod == null)
                {
                    return NotFound($"Method with ID {id} not found.");
                }

                await _sampleMethodService.DeleteAsync(id);
                return Ok("Xóa thành công"); // 2 // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting Method {id}: {ex.Message}");
                return NotFound(ex.Message);
            }

        }

    }
}
