using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public  class RegisterUserDto
    {
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [RegularExpression(
         @"^.*@.*$",
         ErrorMessage = "Email phải chứa ký tự '@'."
     )]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
