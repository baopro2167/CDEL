using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.KitDeliverySS;
using Services.DTO;
namespace WebApplication1.Controllers
{
    [Route("api/KitDelivery")]
    [ApiController]
    public class KitDeliveryController : ControllerBase
    {
        private readonly IKitDeliveryS _kitDeliveryService;
        public KitDeliveryController(IKitDeliveryS kitDeliveryService)
        {
            _kitDeliveryService = kitDeliveryService;
        }

        /// <summary>
        /// Lấy danh sách Kitdeliveries theo id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var delivery = await _kitDeliveryService.GetByIdAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }
            return Ok(delivery);
        }


        /// <summary>
        /// Lấy toàn bộ Kitdeliveries
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deliveries = await _kitDeliveryService.GetAllKitAsync();
            return Ok(deliveries);

        }

        /// <summary>
        /// Lấy danh sách Kitdeliveries có phân trang
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _kitDeliveryService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }


        /// <summary>
        /// Create Kitdeliveries
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddKitDeliveryDTO addKitDeliveryDTO)
        {

            if (addKitDeliveryDTO == null)
            {
                return BadRequest("Kitdeliveries data is required.");
            }

            try
            {
                var addKit = await _kitDeliveryService.AddAsync(addKitDeliveryDTO);
                return CreatedAtAction(nameof(GetById), new { id = addKit.Id }, addKit);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // For validation errors
            }
        }

        /// <summary>
        /// Update Kitdeliveries theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateKitDeliveryDTO updateKitDeliveryDTO)
        {
            if (updateKitDeliveryDTO == null)
            {
                return BadRequest("Kitdeliveries update data is required.");
            }

            try
            {
                var updateKit = await _kitDeliveryService.UpdateAsync(id, updateKitDeliveryDTO);    
                if (updateKit == null)
                {
                    return NotFound($"Kitdeliveries with ID {id} not found.");
                }

                return NoContent(); // 204 No Content, no body
            }

            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message); // e.g., "UpdatedAt must be after CreatedAt."
            }


        }


        /// <summary>
        /// Xóa Kitdeliveries
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var delivery = await _kitDeliveryService.GetByIdAsync(id);
                if (delivery == null)
                {
                    return NotFound($"Kitdeliveries with ID {id} not found.");
                }

                await _kitDeliveryService.DeleteAsync(id);
                return NoContent(); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting Kitdeliveries {id}: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
    }
}
