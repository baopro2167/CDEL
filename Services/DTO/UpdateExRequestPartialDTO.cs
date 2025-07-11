using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdateExRequestPartialDTO
    {
        public int? ServiceId { get; set; } // Nullable để không bắt buộc
        public int? SampleMethodId { get; set; } // Nullable để không bắt buộc
        public DateTime? AppointmentTime { get; set; } // Nullable để không bắt buộc
    }
}
