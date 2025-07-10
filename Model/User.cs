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
    [Table("User")]
   public class User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty!;

        [Required]
        [StringLength(50)]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(200)]
        public string Password { get; set; } = string.Empty;

       
        [StringLength(10)]
        public string? Phone { get; set; } 

       
        [StringLength(20)]
        public string? Address { get; set; } 

        [Required]
        public int RoleId { get; set; }

        public bool Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? RefreshToken { get; set; } = null;
        public DateTime? RefreshTokenExpiryTime { get; set; } = null;
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }


        [JsonIgnore]
        public virtual Role? Role { get; set; }
        public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

        [JsonIgnore]
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        [JsonIgnore]
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        [JsonIgnore]
        public ICollection<ExaminationRequest> ExaminationRequests { get; set; }
    }
}
