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
    [Table("ExaminationResult")]
    public class ExaminationResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        public string? FileUrl { get; set; }

        public DateTime? ResultDate { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateAt { get; set; }
        [JsonIgnore]

        public virtual ExaminationRequest Request { get; set; } = null!;
    }
}
