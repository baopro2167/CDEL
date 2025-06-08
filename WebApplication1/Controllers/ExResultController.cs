using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ExResultSS;
using Services.DTO;
using Model;
namespace WebApplication1.Controllers
{
    [Route("api/ExResult")]
    [ApiController]
    public class ExResultController : ControllerBase
    {
        private readonly IExResultS _exResultService;
        public ExResultController(IExResultS exResultService)
        {
            _exResultService = exResultService;
        }
        /// <summary>
        /// Lấy danh sách exResult theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exResult = await _exResultService.GetByIdAsync(id);
            if (exResult == null) return NotFound();
            return Ok(exResult);
        }

        /// <summary>
        /// Lấy toàn bộ exResult
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kit>>> GetAll()
        {
            var exResult = await _exResultService.GetAllAsync();
            return Ok(exResult);
        }




        /// <summary>
        /// Lấy danh sách exResult có phân trang
        /// </summary>

        [HttpGet("paged")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var results = await _exResultService.GetAll(pageNumber, pageSize);
            return Ok(results);
        }
        /// <summary>
        /// Create exResult
        /// </summary>


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddExResultDTO addExResultDTO)
        {
            if (addExResultDTO == null)
            {
                return BadRequest("exResult data is required.");
            }

            try
            {
                var addExResult = await _exResultService.AddAsync(addExResultDTO);
                return CreatedAtAction(nameof(GetById), new { id = addExResult.Id }, addExResult);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // For validation errors
            }
        }

        /// <summary>
        /// Update exResult theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateExResultDTO updateExResultDTO)
        {
            if (updateExResultDTO == null)
            {
                return BadRequest("exResult update data is required.");
            }

            try
            {
                var updateExResult = await _exResultService.UpdateAsync(id, updateExResultDTO);
                if (updateExResult == null)
                {
                    return NotFound($"exResult with ID {id} not found.");
                }

                return NoContent(); // 204 No Content, no body
            }

            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message); // e.g., "UpdatedAt must be after CreatedAt."
            }


        }
        /// <summary>
        /// Xóa exResult
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var exResult = await _exResultService.GetByIdAsync(id);
                if (exResult == null)
                {
                    return NotFound($"exResult with ID {id} not found.");
                }

                await _exResultService.DeleteAsync(id);
                return NoContent(); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting exResult {id}: {ex.Message}");
                return NotFound(ex.Message);
            }

        }
    }
}
