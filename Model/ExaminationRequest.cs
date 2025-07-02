using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

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
        public String StatusId { get; set; }

        
      

        [Required]
        public DateTime AppointmentTime { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateAt { get; set; }

        public int StaffId { get; set; } = 0;



        // Navigation propertiess
        [JsonIgnore]

        public virtual User? User { get; set; }


        [JsonIgnore]
        public virtual Service Service { get; set; } = null!;
        [JsonIgnore]
        public virtual SampleMethod SampleMethod { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<ExaminationResult> ExaminationResults { get; set; } = new List<ExaminationResult>();
        [JsonIgnore]
        public virtual ICollection<KitDelivery> KitDeliveries { get; set; } = new List<KitDelivery>();
    }
}

