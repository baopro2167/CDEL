using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public  class ServiceSampleMethod
    {
        [ForeignKey("ServiceId")]
        public int ServiceId { get; set; }
        
        public Service Service { get; set; }  // Mối quan hệ với Service
        [ForeignKey("SampleMethodId")]

        public int SampleMethodId { get; set; }
        public SampleMethod SampleMethod { get; set; }  // Mối quan hệ với SampleMethod
    }
}
