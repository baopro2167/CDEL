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
    [Table("BlogPost")]
    public class BlogPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

       
        public string? Author { get; set; }
        
        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateAt { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }

       // public virtual ICollection<KitDelivery> KitDeliveries { get; set; } = null!;
    }
}
