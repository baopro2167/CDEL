using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdateServiceBDTO
    {
      
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

      
        public string Type { get; set; } = null!;

       
        public decimal Price { get; set; }

      

     
      
    }
}
