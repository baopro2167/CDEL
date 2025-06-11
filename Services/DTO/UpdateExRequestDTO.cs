using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdateExRequestDTO
    {
      

        public int ServiceId { get; set; }


        public int PriorityId { get; set; }


        public int SampleMethodId { get; set; }


        public Boolean StatusId { get; set; }





        public DateTime AppointmentTime { get; set; }


        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;


    }
}
