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

    [Table("Kit")]
    public class Kit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateAt { get; set; }

        [JsonIgnore]
        public virtual ICollection<KitDelivery> KitDeliveries { get; set; } = new List<KitDelivery>();
    }
}
