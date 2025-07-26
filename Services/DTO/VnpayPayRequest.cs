using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class VnpayPayRequest
    {
        private readonly IHttpContextAccessor? httpContextAccessor;

        public SortedList<string, string> requestData = new SortedList<string, string>(new VnpayCompare());

        // Property IP động lấy từ HttpContext
        public string? IpAddress => httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        public VnpayPayRequest() { }

        public VnpayPayRequest(IHttpContextAccessor accessor)
        {
            this.httpContextAccessor = accessor;
        }

        public VnpayPayRequest(string version, string tmnCode, DateTime createDate, string ipAddress,
            decimal amount, string currCode, string orderType, string orderInfo,
            string returnUrl, string txnRef)
        {
            requestData["vnp_Version"] = version;
            requestData["vnp_Command"] = "pay";
            requestData["vnp_TmnCode"] = tmnCode;
            requestData["vnp_Amount"] = ((int)amount * 100).ToString();
            requestData["vnp_CurrCode"] = currCode;
            requestData["vnp_TxnRef"] = txnRef;
            requestData["vnp_OrderInfo"] = orderInfo;
            requestData["vnp_OrderType"] = orderType;
            requestData["vnp_ReturnUrl"] = returnUrl;
            requestData["vnp_IpAddr"] = ipAddress; // truyền ip hoặc lấy từ property IpAddress
            requestData["vnp_CreateDate"] = createDate.ToString("yyyyMMddHHmmss");
            requestData["vnp_Locale"] = "vn";
        }

        public string GetLink(string VnpUrl, string secretKey)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            string rawData = data.ToString().TrimEnd('&');
            string secureHash = HashHelper.HmacSHA512(secretKey, rawData);
            return $"{VnpUrl}?{rawData}&vnp_SecureHash={secureHash}";
        }
        public class VnpayCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                // So sánh thứ tự theo bảng chữ cái (Ordinal)
                return string.CompareOrdinal(x, y);
            }
        }
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
    } 
}
