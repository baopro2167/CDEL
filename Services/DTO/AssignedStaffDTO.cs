using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AssignedStaffDTO
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = default!;
    }
}
