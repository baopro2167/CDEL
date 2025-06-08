using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdateExResultDTO
    {
        

        public string? FileUrl { get; set; }

        public DateTime? ResultDate { get; set; }

       

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;


    }
}
