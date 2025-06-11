using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
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

        public DateTime? SentAt { get; set; }

        public DateTime? ReceivedAt { get; set; }

        public Boolean? StatusId { get; set; }

        public virtual ExaminationRequest Request { get; set; } = null!;
        public virtual Kit Kit { get; set; } = null!;

        
    }
}
