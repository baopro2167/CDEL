using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AddKitDTO
    {
        public int Id { get; set; }

      
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

       
        public DateTime UpdateAt { get; set; }

        
    }
}
