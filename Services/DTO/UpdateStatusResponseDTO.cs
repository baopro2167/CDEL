﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public  class UpdateStatusResponseDTO
    {
        public int UserId { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
