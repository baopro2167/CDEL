using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AddExRequestDTO
    {
        public int UserId { get; set; }

       
        public int ServiceId { get; set; }

     
        public int PriorityId { get; set; }

   
        public int SampleMethodId { get; set; }

   
        public bool StatusId { get; set; }


        
        public DateTime AppointmentTime { get; set; }

 
       

   
     
    }
}
