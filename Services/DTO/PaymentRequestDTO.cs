using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class PaymentRequestDTO
    {
        public int UserId { get; set; }
        public int RequestId { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }
    }
}
