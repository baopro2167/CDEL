using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.RatingSS;

namespace WebApplication1.Controllers
{
    [Route("api/Rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        /// <summary>
        /// lấy toàn bộ rating có phân trang
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ratings = await _ratingService.GetAll(pageNumber, pageSize);
            return Ok(ratings);
        }
        /// <summary>
        /// lấy rating theo id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await _ratingService.GetByIdAsync(id);
            if (rating == null) return NotFound();
            return Ok(rating);
        }
        /// <summary>
        /// Tạo rating
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] AddRatingDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var rating = await _ratingService.AddRatingAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = rating.Id }, rating);
        }
        /// <summary>
        /// Cập nhật rating theo id
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] UpdateRatingDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var rating = await _ratingService.UpdateRatingAsync(id, dto);
            if (rating == null) return NotFound();
            return Ok(rating);
        }
        /// <summary>
        /// Xóa rating theo id
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            await _ratingService.DeleteRatingAsync(id);
            return NoContent();
        }
        /// <summary>
        /// Tìm rating theo userid
        /// </summary>
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ratings = await _ratingService.GetByUserIdAsync(userId, pageNumber, pageSize);
            return Ok(ratings);
        }
        /// <summary>
        /// Tìm rating theo requestId
        /// </summary>
        [HttpGet("by-request/{requestId}")]
        public async Task<IActionResult> GetByRequestId(int requestId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ratings = await _ratingService.GetByRequestIdAsync(requestId, pageNumber, pageSize);
            return Ok(ratings);
        }
    }
}
