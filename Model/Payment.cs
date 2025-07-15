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
    [Table("Payment")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RequestId { get; set; }
        [Required]
        public decimal Amount { get; set; }
       
       

        
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string TransactionNo { get; set; } = string.Empty;

      
        public string ResponseCode { get; set; } = string.Empty;


        public string StatusId { get; set; } = "Pending";


        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

       
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ExaminationRequest Request { get; set; } 
    }
}
