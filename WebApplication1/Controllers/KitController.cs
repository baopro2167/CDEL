using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;
using Services.DTO;
using Services.KitS;
using System.Net;
namespace WebApplication1.Controllers
{
    [Route("api/Kit")]
    [ApiController]
    public class KitController : ControllerBase
    {
        private readonly IKitService _KitService;

        public KitController (IKitService kitService)
        {
            _KitService = kitService;
        }

        /// <summary>
        /// Lấy danh sách Kit theo id
        /// </summary>
        [Authorize(Roles = "3,4")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Kit>> GetById(int id)
        {
            var kit = await _KitService.GetByIdAsync(id);
            if (kit == null) return NotFound();
            return Ok(kit);
        }
        /// <summary>
        /// Lấy toàn bộ Kit
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kit>>> GetAll()
        {
            var kit = await _KitService.GetAllKitAsync();
            return Ok(kit);
        }
        /// <summary>
        /// Lấy danh sách kit có phân trang
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _KitService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }




        /// <summary>
        /// Create Kit
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Kit>> Create([FromBody] AddKitDTO addKitDTO)
        {
            if (addKitDTO == null)
            {
                return BadRequest("Kit data is required.");
            }

            try
            {
                var addKit = await _KitService.AddKitAsync(addKitDTO);
                return CreatedAtAction(nameof(GetById), new { id = addKit.Id }, addKit);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // For validation errors
            }
        }
        /// <summary>
        /// Update kit theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateKitDTO updateKitDTO)
        {
            if (updateKitDTO == null)
            {
                return BadRequest("KIT update data is required.");
            }

            try
            {
                var updateKit = await _KitService.UpdateKitAsync(id, updateKitDTO);
                if (updateKit == null)
                {
                    return NotFound($"KIT with ID {id} not found.");
                }

                return Ok("Update kit thành công"); 
            }

            catch (ArgumentException ex)
            {
                
                return BadRequest(ex.Message); // e.g., "UpdatedAt must be after CreatedAt."
            }


        }

        /// <summary>
        /// Xóa Kit
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var address = await _KitService.GetByIdAsync(id);
                if (address == null)
                {
                    return NotFound($"Kit with ID {id} not found.");
                }

                await _KitService.DeleteKitAsync(id);
                return Ok("Xóa thành công"); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting address {id}: {ex.Message}");
                return NotFound(ex.Message);
            }

        }



    }
}
