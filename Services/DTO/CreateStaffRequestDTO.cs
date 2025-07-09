using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class CreateStaffRequestDTO
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;

        public int UserId { get; set; } // Assuming UserId is part of the request DTO

    }
}
