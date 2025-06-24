using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class GetAllUserResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } 
        public bool Status { get; set; } 
    }
}
