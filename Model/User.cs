using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
    
{
    [Table("User")]
   public class User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(20)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string Phone { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Address { get; set; } = null!;

        [Required]
        public int RoleId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }

        public virtual Role? Role { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        public ICollection<ExaminationRequest> ExaminationRequests { get; set; }
    }
}
