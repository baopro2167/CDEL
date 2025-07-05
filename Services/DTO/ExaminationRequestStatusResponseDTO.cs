using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ExaminationRequestStatusResponseDTO
    {
        public int RequestId { get; set; }
        public string Status { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
