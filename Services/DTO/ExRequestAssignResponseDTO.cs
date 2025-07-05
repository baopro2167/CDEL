using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ExRequestAssignResponseDTO
    {
        public int RequestId { get; set; }
        public AssignedStaffDTO AssignedStaff { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
