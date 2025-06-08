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
        /// Lấy toàn bộ ExRequest
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExaminationRequest>>> GetAll()
        {
            var ExRequestS = await _exRequestService.GetAllAsync();
            return Ok(ExRequestS);
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
        /// Create ExRequest
        /// </summary>


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

            return NoContent(); // 204 No Content, no body





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
                return NoContent(); // 204 No Content indicates successful deletion
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
