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

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var url = await _paymentService.CreatePaymentUrl(request);
            return Ok(new { paymentUrl = url });
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
