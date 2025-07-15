using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CreatePaymentRequest
    {
        public int UserId { get; set; }
        public int RequestId { get; set; }
        public decimal Amount { get; set; } 

        public string OrderInfo { get; set; } = string.Empty;

        public string Locale { get; set; } = "vn";
        public string OrderType { get; set; } = "other";
    }
}
