using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ServiceGetDTO
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<SampleMethodDTO> SampleMethods { get; set; } = new List<SampleMethodDTO>();
    }
}
