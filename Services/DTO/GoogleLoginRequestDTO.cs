using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public  class GoogleLoginRequestDTO
    {
        /// <summary>
        /// Google ID token lấy từ client
        /// </summary>
        public string IdToken { get; set; } = null!;
    }
}
