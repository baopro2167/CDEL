using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdateUserProfileDTO
    {
        public string Name { get; set; } = string.Empty!;

       
        public string? Phone { get; set; }

       
        public string? Address { get; set; }
    }
}
