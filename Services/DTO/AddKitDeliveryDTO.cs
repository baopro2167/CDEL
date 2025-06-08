using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AddKitDeliveryDTO
    {
     

        public int RequestId { get; set; }


        public int KitId { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public DateTime ReceivedAt { get; set; }

        public int? StatusId { get; set; }



    }
}
