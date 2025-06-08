using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("ExaminationRequest")]
    public  class ExaminationRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int PriorityId { get; set; }

        [Required]
       public int SampleMethodId { get; set; }

        [Required]
        public int StatusId { get; set; }

        
      

        [Required]
        public DateTime AppointmentTime { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateAt { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
    

       
        public virtual Service Service { get; set; } = null!;
        public virtual SampleMethod SampleMethod { get; set; } = null!;
        public virtual ICollection<ExaminationResult> ExaminationResults { get; set; } = new List<ExaminationResult>();
        public virtual ICollection<KitDelivery> KitDeliveries { get; set; } = new List<KitDelivery>();
    }
}

