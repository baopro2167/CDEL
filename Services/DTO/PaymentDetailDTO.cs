using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public  class PaymentDetailDTO
    {

        public int PaymentId { get; set; }
        public int RequestId { get; set; }
        public decimal Amount { get; set; }
        public string StatusId { get; set; }
        public string TransactionNo { get; set; }
        public string ResponseCode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

       
       

        // Tên service & sample method
        public string ServiceName { get; set; }
        public string SampleMethodName { get; set; }
    }
}
