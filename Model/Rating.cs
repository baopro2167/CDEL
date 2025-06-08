using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Rating")]
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int RatingValue { get; set; }

        [StringLength(500)]
        public string? Feedback { get; set; } = null;

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual User? User { get; set; }

        public virtual ExaminationRequest? Request { get; set; } = null!;
    }
}
