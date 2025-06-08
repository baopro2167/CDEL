using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class UpdateKitDTO
    {
       


        public string Name { get; set; } = null!;

        public string? Description { get; set; }


        

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
