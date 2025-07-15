using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.PaymentSS;

namespace WebApplication1.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // 1. Tạo thanh toán mới
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            if (request == null || request.Amount <= 0)
                return BadRequest(new { message = "Invalid payment request" });

            try
            {
                var paymentUrl = await _paymentService.CreatePaymentUrl(
                    request.UserId,
                    request.RequestId,
                    request.Amount,
                    request.OrderInfo
                );

                return Ok(new { paymentUrl });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Create payment failed", error = ex.Message });
            }
        }

        [HttpGet("payment-return")]
        public IActionResult PaymentReturn()
        {
            if (!_paymentService.ValidateVNPaySignature(Request.Query))
            {
                return BadRequest("Chữ ký VNPay không hợp lệ");
            }
            {
                return BadRequest("Chữ ký VNPay không hợp lệ");
            }
           
           

            // Tiếp tục xử lý: cập nhật trạng thái thanh toán, hiển thị kết quả...
            return Ok("Xác minh chữ ký thành công");
        }
    }
}
