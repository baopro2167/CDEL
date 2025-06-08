using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class UpdateKitDeliveryDTO
    {
       public DateTime ReceivedAt { get; set; } =DateTime.UtcNow;

        public int StatusId { get; set; }


    }
}
