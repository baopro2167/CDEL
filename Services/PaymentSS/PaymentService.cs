using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.Pkcs;
using Repositories.ExRequestRepo;
using Repositories.Pagging;
using Repositories.PaymentRepo;
using Repositories.SampleMethodRepo;
using Repositories.ServiceRepo;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Services.DTO.VnpayPayRequest;

namespace Services.PaymentSS
{
    public class PaymentService : IPaymentService
    {
        private readonly IHostEnvironment _env;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IExRequestRepository _reqRepo;
        private readonly IServiceRepository _svcRepo;
        private readonly ISampleMethodRepository _smRepo;
        private readonly string _vnpayUrl;
        private readonly string _vnpTmnCode;
        private readonly string _vnpHashSecret;
        private readonly string _vnpReturnUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SortedList<string, string> requestData = new SortedList<string, string>(new VnpayCompare()); // Giữ nguyên từ VnpayPayRequest

        // Property IP động lấy từ HttpContext
        public string? IpAddress => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        public PaymentService(IPaymentRepository paymentRepository, IConfiguration configuration
              , IHttpContextAccessor httpContextAccessor, IExRequestRepository reqRepo,
            IServiceRepository svcRepo,
            ISampleMethodRepository smRepo ,
             IHostEnvironment env )
        {
            _env = env;
            _reqRepo = reqRepo;
            _svcRepo = svcRepo;
            _smRepo = smRepo;
            _paymentRepository = paymentRepository;
            _vnpayUrl = configuration["VNPaySettings:VnpUrl"]; // Sử dụng sandbox cho test
            _vnpTmnCode = configuration["VNPaySettings:VnpTmnCode"];
            _vnpHashSecret = configuration["VNPaySettings:VnpHashSecret"];
            _vnpReturnUrl = configuration["VNPaySettings:VnpReturnUrl"];
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PaymentDetailDTO?> GetPaymentDetailAsync(int paymentId)
        {
            var p = await _paymentRepository.GetByIdAsync(paymentId);
            if (p == null) return null;

            var er = await _reqRepo.GetByIdAsync(p.RequestId);
            if (er == null) return null;

            var svc = await _svcRepo.GetByIdAsync(er.ServiceId);
            var sm = await _smRepo.GetByIdAsync(er.SampleMethodId);

            return new PaymentDetailDTO
            {
                PaymentId = p.Id,
                RequestId = p.RequestId,
                Amount = p.Amount,
                StatusId = p.StatusId,
                TransactionNo = p.TransactionNo ?? "",
                ResponseCode = p.ResponseCode ?? "",
                PaymentDate = p.PaymentDate,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,

              
                ServiceName = svc?.Name ?? "",
                SampleMethodName = sm?.Name ?? ""
            };
        }



        public async Task<string> CreatePaymentUrl(int userId, int requestId, decimal amount, string orderInfo)
        {
            var payment = new Model.Payment
            {
                UserId = userId,
                RequestId = requestId,
                Amount = amount,
                StatusId = "Pending",
                CreatedAt = GetVietnamTime()
            };

            await _paymentRepository.CreateAsync(payment);

            // Điền dữ liệu trực tiếp vào requestData
            requestData["vnp_Version"] = "2.1.0";
            requestData["vnp_Command"] = "pay";
            requestData["vnp_TmnCode"] = _vnpTmnCode;
            requestData["vnp_Amount"] = ((int)amount * 100).ToString();
            requestData["vnp_CurrCode"] = "VND";
            requestData["vnp_TxnRef"] = payment.Id.ToString();
            requestData["vnp_OrderInfo"] = orderInfo;
            requestData["vnp_OrderType"] = "other";
            requestData["vnp_ReturnUrl"] = _vnpReturnUrl;
            requestData["vnp_IpAddr"] = IpAddress ?? "127.0.0.1";
            requestData["vnp_CreateDate"] = GetVietnamTime().ToString("yyyyMMddHHmmss");
            requestData["vnp_Locale"] = "vn";

            string rawData = BuildRawData(requestData);
            string secureHash = GenerateSecureHash(rawData);
            return $"{_vnpayUrl}?{rawData}&vnp_SecureHash={secureHash}";
        }
        public async Task<(bool IsValid, string Status, string TransactionNo)> ProcessVnpayReturn(
    IQueryCollection query)
        {
            // 1. Lấy khoản secure hash
            var vnpSecureHash = query["vnp_SecureHash"].ToString();
            var vnpSecureHashType = query["vnp_SecureHashType"].ToString(); // nếu có

            // 2. Build sorted list từ query
            var sorted = new SortedList<string, string>(StringComparer.Ordinal);
            foreach (var key in query.Keys)
            {
                if (key.StartsWith("vnp_", StringComparison.Ordinal) &&
                    key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                {
                    sorted[key] = query[key];
                }
            }

            // 3. Build raw data
            var rawData = new StringBuilder();
            foreach (var kv in sorted)
            {
                rawData.Append(WebUtility.UrlEncode(kv.Key))
                       .Append('=')
                       .Append(WebUtility.UrlEncode(kv.Value))
                       .Append('&');
            }
            rawData.Length--; // bỏ ký tự '&' cuối

            // 4. Tính hash
            var computedHash = HashHelper.HmacSHA512(_vnpHashSecret, rawData.ToString());
            if (!string.Equals(computedHash, vnpSecureHash, StringComparison.OrdinalIgnoreCase))
                return (false, null, null);

            // 5. Xác thực và cập nhật trạng thái
            int paymentId = int.Parse(query["vnp_TxnRef"]);
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) return (false, null, null);

            var responseCode = query["vnp_ResponseCode"].ToString();
            payment.StatusId = responseCode == "00" ? "Success" : "Failed";
            payment.TransactionNo = query["vnp_TransactionNo"];
            payment.ResponseCode = responseCode;
            payment.PaymentDate = GetVietnamTime();
            payment.UpdatedAt = GetVietnamTime();
            await _paymentRepository.UpdateAsync(payment);

            return (true, payment.StatusId, payment.TransactionNo);
        }

        // Xây dựng dữ liệu thô để tạo hash
        private string BuildRawData(SortedList<string, string> requestData)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            return data.ToString().TrimEnd('&');
        }

        private string GenerateSecureHash(string rawData)
        {
            return HashHelper.HmacSHA512(_vnpHashSecret, rawData);
        }

        // Lớp VnpayCompare
        public class VnpayCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.CompareOrdinal(x, y);
            }
        }

        // Lớp HashHelper
        public static class HashHelper
        {
            public static string HmacSHA512(string key, string inputData)
            {
                var encoding = new System.Text.UTF8Encoding();
                byte[] keyBytes = encoding.GetBytes(key);
                byte[] messageBytes = encoding.GetBytes(inputData);
                using (var hmacsha512 = new System.Security.Cryptography.HMACSHA512(keyBytes))
                {
                    byte[] hashmessage = hmacsha512.ComputeHash(messageBytes);
                    return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
                }
            }
        }

        // Lấy thời gian theo múi giờ Việt Nam (UTC+7)
        private DateTime GetVietnamTime()
        {
            TimeZoneInfo vietnamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Múi giờ Việt Nam
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamZone);
        }


        public async Task<PaginatedList<Payment>> GetPaymentsByUserAsync(
            int userId, int pageNumber, int pageSize)
        {
            // Lấy PaginatedList<Payment>
            var paged = await _paymentRepository
                .GetByUserIdAsync(userId, pageNumber, pageSize);

            // Map sang DTO
            var dtoItems = paged.Items.Select(p => new Payment
            {
                Id = p.Id,
                UserId = p.UserId,
                RequestId = p.RequestId,
                Amount = p.Amount,
                StatusId = p.StatusId,
                TransactionNo = p.TransactionNo,
                ResponseCode = p.ResponseCode,
                PaymentDate = p.PaymentDate,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            // Trả về PaginatedList<PaymentResponseDTO>
            return new PaginatedList<Payment>(
                dtoItems,
                paged.TotalCount,
                paged.PageNumber,
                paged.PageSize
            );
        }




    }
}

