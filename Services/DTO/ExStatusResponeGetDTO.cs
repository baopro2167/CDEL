using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ExStatusResponeGetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
      
        public int ServiceId { get; set; }
    
        public int SampleMethodId { get; set; }
       
        public string StatusId { get; set; }
        public string StatusName { get; set; } // Thêm thuộc tính mới
        public DateTime? AppointmentTime { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int StaffId { get; set; }
       
    }
}
