using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model;
using Repositories.PaymentRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PaymentSS
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly string _vnpayUrl;
        private readonly string _vnpTmnCode;
        private readonly string _vnpHashSecret;
        private readonly string _vnpReturnUrl;
        public PaymentService(IPaymentRepository paymentRepository, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _vnpayUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; // Sử dụng sandbox cho test
            _vnpTmnCode = configuration["VNPaySettings:VnpTmnCode"];
            _vnpHashSecret = configuration["VNPaySettings:VnpHashSecret"];
            _vnpReturnUrl = configuration["VNPaySettings:VnpReturnUrl"];
        }
        public async Task<string> CreatePaymentUrl(int userId, int requestId, decimal amount, string orderInfo)
        {
            var payment = new Payment
            {
                UserId = userId,
                RequestId = requestId,
                Amount = amount,
                StatusId = "Pending",
                 ResponseCode = "Pending",
                 TransactionNo= "Pending",

            };

            await _paymentRepository.CreateAsync(payment);

            string vnp_TxnRef = payment.Id.ToString(); // Sử dụng Id làm transaction reference
            string vnp_IpAddr = "127.0.0.1"; // Cần lấy IP thực tế từ request
            string vnp_CreateDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            var vnp_Params = new Dictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", _vnpTmnCode },
                { "vnp_Amount", (amount * 100).ToString() }, // VNPay yêu cầu đơn vị nhỏ nhất (VND)
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", vnp_TxnRef },
                { "vnp_OrderInfo", orderInfo },
                { "vnp_OrderType", "other" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", _vnpReturnUrl },
                { "vnp_IpAddr", vnp_IpAddr },
                { "vnp_CreateDate", vnp_CreateDate }
            };

            var expireDate = DateTime.Now.AddMinutes(15);
            vnp_Params.Add("vnp_ExpireDate", expireDate.ToString("yyyyMMddHHmmss"));

            var fieldNames = vnp_Params.Keys.ToList();
            fieldNames.Sort();
            var hashData = string.Join("&", fieldNames.Select(key => $"{key}={vnp_Params[key]}"));
            var vnp_SecureHash = HmacSHA512(_vnpHashSecret, hashData);

            vnp_Params.Add("vnp_SecureHash", vnp_SecureHash);

            var queryString = string.Join("&", vnp_Params.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{_vnpayUrl}?{queryString}";
        }
        public bool ValidateVNPaySignature(IQueryCollection query)
        {
            var vnpData = query
                .Where(kv => kv.Key.StartsWith("vnp_") && kv.Key != "vnp_SecureHash")
             
            .ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

            var sortedKeys = vnpData.Keys.OrderBy(k => k).ToList();
            var rawData = string.Join("&", sortedKeys.Select(k => $"{k}={vnpData[k]}"));
            var checkHash = HmacSHA512(_vnpHashSecret, rawData);

            return checkHash.Equals(query["vnp_SecureHash"], StringComparison.OrdinalIgnoreCase);
        }
        private static string HmacSHA512(string key, string inputData)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new System.Security.Cryptography.HMACSHA512(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public async Task<string> UpdatePaymentUrlAsync(int paymentId)
{
    var payment = await _paymentRepository.GetByIdAsync(paymentId);
    if (payment == null)
    {
        throw new Exception("Payment not found");
    }

    if (payment.StatusId == "Success")
    {
        throw new Exception("Cannot update a successful payment");
    }

    string vnp_TxnRef = payment.Id.ToString(); // vẫn giữ nguyên transaction ref cũ
    string vnp_IpAddr = "127.0.0.1"; // nên lấy IP thực
    string vnp_CreateDate = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

    var vnp_Params = new Dictionary<string, string>
    {
        { "vnp_Version", "2.1.0" },
        { "vnp_Command", "pay" },
        { "vnp_TmnCode", _vnpTmnCode },
        { "vnp_Amount", ((int)(payment.Amount * 100)).ToString() },
        { "vnp_CurrCode", "VND" },
        { "vnp_TxnRef", vnp_TxnRef },
        { "vnp_OrderInfo", $"Thanh toán lại đơn #{payment.Id}" },
        { "vnp_OrderType", "other" },
        { "vnp_Locale", "vn" },
        { "vnp_ReturnUrl", _vnpReturnUrl },
        { "vnp_IpAddr", vnp_IpAddr },
        { "vnp_CreateDate", vnp_CreateDate }
    };

    var expireDate = DateTime.UtcNow.AddMinutes(15);
    vnp_Params.Add("vnp_ExpireDate", expireDate.ToString("yyyyMMddHHmmss"));

    var fieldNames = vnp_Params.Keys.ToList();
    fieldNames.Sort();
    var hashData = string.Join("&", fieldNames.Select(key => $"{key}={vnp_Params[key]}"));
    var vnp_SecureHash = HmacSHA512(_vnpHashSecret, hashData);

    vnp_Params.Add("vnp_SecureHash", vnp_SecureHash);

    var queryString = string.Join("&", vnp_Params.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
    var newUrl = $"{_vnpayUrl}?{queryString}";

    // (Tùy chọn) Cập nhật lại updatedAt
    payment.UpdatedAt = DateTime.UtcNow;
    await _paymentRepository.UpdateAsync(payment);

    return newUrl;
}

       
    }
}
