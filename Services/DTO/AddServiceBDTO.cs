﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class AddServiceBDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

      
        public string ?Type { get; set; } 

       
        public decimal Price { get; set; }


        public List<int> SampleMethodIds { get; set; }



    }
}
