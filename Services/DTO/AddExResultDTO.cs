using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AddExResultDTO
    {
        public int RequestId { get; set; }

        public string? FileUrl { get; set; }

        public DateTime? ResultDate { get; set; }


        public DateTime CreateAt { get; set; } = DateTime.UtcNow;


        



    }
}
