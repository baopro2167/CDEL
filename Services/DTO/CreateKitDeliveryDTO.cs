using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CreateKitDeliveryDTO
    {
        public int RequestId { get; set; }
        public int StaffId { get; set; }
        public string KitType { get; set; } = default!;
    }
}
