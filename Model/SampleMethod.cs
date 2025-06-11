using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    [Table("SampleMethod")]
    public class SampleMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<ServiceSampleMethod> ServiceSampleMethods { get; set; } = new List<ServiceSampleMethod>();
    }
}
