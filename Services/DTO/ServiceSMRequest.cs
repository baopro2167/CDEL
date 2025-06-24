using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ServiceSMRequest
    {
        public int Id { get; set; }


       
        public int ServiceId { get; set; }


        
        public int SampleMethodId { get; set; }

    }
}
