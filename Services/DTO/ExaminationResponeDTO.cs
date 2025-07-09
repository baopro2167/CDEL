using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ExaminationResponeDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public int SampleMethodId { get; set; }
        public string SampleMethodName { get; set; }
        public string StatusId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public DateTime CreateAt { get; set; }
      
        public int? StaffId { get; set; }
    }
}
