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
    [Table("ServiceSampleMethod")]
    public class ServiceSampleMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        [ForeignKey("ServiceId")]
        public int ServiceId { get; set; }

       
        [ForeignKey("SampleMethodId")]
        public int SampleMethodId { get; set; }

        public virtual Service Service { get; set; } = null!;

        [JsonIgnore]
        public virtual SampleMethod SampleMethod { get; set; } = null!;
    }
}
