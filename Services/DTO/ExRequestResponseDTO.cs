using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ExRequestResponseDTO
    {
        public int RequestId { get; set; }
        public string Status { get; set; } = default!;
        public string StatusName { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
