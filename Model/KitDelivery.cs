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
    [Table("KitDelivery")]
   public  class KitDelivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int? RequestId { get; set; }

        [Required]
        public int KitId { get; set; }

        public DateTime? SentAt { get; set; }   = DateTime.UtcNow;

        public DateTime? ReceivedAt { get; set; } = DateTime.UtcNow;

        public String StatusId { get; set; }
        [JsonIgnore]
        public virtual ExaminationRequest Request { get; set; } = null!;
        [JsonIgnore]
        public virtual Kit Kit { get; set; } = null!;

        
    }
}
