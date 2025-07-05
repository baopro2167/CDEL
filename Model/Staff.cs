using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model
{
    public class Staff
    {
        public int Id { get; set; }
        public string FullName { get; set; }
       
        public string Email { get; set; }

        [ForeignKey("UserId")]
        public int? UserId { get; set; }

        [JsonIgnore]

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
