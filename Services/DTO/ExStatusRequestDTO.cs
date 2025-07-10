using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ExStatusRequestDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = default!;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = default!;
       


        public int PriorityId { get; set; }

        public int SampleMethodId { get; set; }

        public string SampleMethodName { get; set; } = default!;
        public string StatusId { get; set; }

 
        public DateTime AppointmentTime { get; set; }

   
        public DateTime CreateAt { get; set; }


        public DateTime UpdateAt { get; set; }

        public int StaffId { get; set; }

        public string StaffName { get; set; } = default!;

        // — MỚI THÊM —

    }
}
