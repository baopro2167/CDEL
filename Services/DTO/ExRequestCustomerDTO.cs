using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ExRequestCustomerDTO
    {
        public int RequestId { get; set; }
        public string? ServiceName { get; set; }
        public Boolean StatusId { get; set; }
       
    }
}
