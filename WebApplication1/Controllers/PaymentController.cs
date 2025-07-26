using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repositories.Pagging;
using Services.DTO;
using Services.PaymentSS;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace WebApplication1.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentController( IConfiguration config, IHttpContextAccessor httpContextAccessor,
            IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("vnpay")]
        public async Task<IActionResult> CreateVnpayPayment([FromBody] PaymentRequestDTO request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest(new { error = "Invalid payment request" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentUrl = await _paymentService.CreatePaymentUrl(request.UserId, request.RequestId, request.Amount, request.OrderInfo ?? "Payment for request");
            return Ok(new { PaymentUrl = paymentUrl });
        }


        /// <summary>
        /// Xử lý phản hồi từ VNPay (Return URL)
        /// </summary>
        [HttpGet("payment-return")]
        public async Task<IActionResult> HandleVnpayReturn()
        {
            // Lấy toàn bộ query string đã được VNPay gửi về
            var query = HttpContext.Request.Query;

            // Gọi service để xác thực hash và cập nhật payment record
            var (isValid, status, transactionNo) =
                await _paymentService.ProcessVnpayReturn(query);

            if (!isValid)
                return BadRequest(new { error = "Invalid secure hash or payment not found" });

            if (status == "Success")
                return Ok(new { Message = "Payment successful", TransactionId = transactionNo });

            return Ok(new { Message = "Payment failed", ResponseCode = status });
        }

        // Cập nhật trạng thái thanh toán


        /// <summary>
        /// Lấy danh sách payments theo userId, phân trang
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUser(
    int userId, int pageNumber, int pageSize)
        { 
            PaginatedList<Payment> result =
                await _paymentService.GetPaymentsByUserAsync(userId, pageNumber, pageSize);

            if (!result.Items.Any())
                return NotFound(new { message = $"Không có payment cho userId = {userId}" });

            

            return Ok(result);
        }
        /// <summary>
        /// Lấy chi tiết payment kèm thông tin service và sample method
        /// </summary>
        [HttpGet("details/{paymentId}")]
        public async Task<IActionResult> GetPaymentDetails(int paymentId)
        {
            PaymentDetailDTO? dto = await _paymentService.GetPaymentDetailAsync(paymentId);
            if (dto == null)
                return NotFound(new { message = $"PaymentId = {paymentId} not found" });

            return Ok(dto);
        }



    }

}

