using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class KitDeliveryStatusResponseDTO
    {
        public int KitDeliveryId { get; set; }
        public string Status { get; set; } = default!;
        public DateTime ReceivedAt { get; set; }
    }
}
