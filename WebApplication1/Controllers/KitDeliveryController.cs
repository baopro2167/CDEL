using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.KitDeliverySS;
using Services.DTO;
using Microsoft.AspNetCore.Authorization;
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
        /// User xác nhận đã nhận hoặc đã gửi lại (“Sent”→“Received”/“Returned”)
        /// </summary>
        [HttpPatch("{kitDeliveryId}/acknowledge")]
       
       
        public async Task<ActionResult<UpdateKitDeliverySSResponseDTO>> Acknowledge(
            int kitDeliveryId,
            [FromBody] UpdateKitDeliverySSDTO dto)
        {
            if (dto == null)
                return BadRequest("Request body must contain status.");

            try
            {
                var result = await _kitDeliveryService.AcknowledgeAsync(kitDeliveryId, dto);
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
        /// Đánh dấu đơn giao kit là Sent (từ Pending → Sent) mà không cần truyền body
        /// </summary>
        [HttpPatch("{kitDeliveryId}/status")]
        [Authorize(Roles = "3")]

        public async Task<ActionResult<KitDeliveryStatusResponseDTO>> MarkAsSent(int kitDeliveryId)
        {
            try
            {
                var result = await _kitDeliveryService.MarkAsSentAsync(kitDeliveryId);
                // thành công: trả nguyên DTO
                return Ok(result);
            }
            catch (KeyNotFoundException knf)
            {
                // 404 với plain string
                return NotFound(knf.Message);
            }
            catch (InvalidOperationException inv)
            {
                // 400 với plain string
                return BadRequest(inv.Message);
            }

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
        /// STAFF Create Kitdeliveries
        /// </summary>

        [HttpPost]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Add([FromBody] AddKitDeliveryDTO addKitDeliveryDTO)
        {

            if (addKitDeliveryDTO == null)
            {
                return BadRequest("Kitdeliveries data is required.");
            }

            try
            {
                var addKit = await _kitDeliveryService.AddAsync(addKitDeliveryDTO);
                return CreatedAtAction(nameof(GetById), new { id = addKit.KitDeliveryId }, addKit);
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

                return Ok("Update Kitdeliveries thành công");
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
