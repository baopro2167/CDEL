using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class TokenResponseDTO
    {
        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }
        public int RoleId { get; set; }
    }
}
