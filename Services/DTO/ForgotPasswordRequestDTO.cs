﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ForgotPasswordRequestDTO
    {
        public string Email { get; set; } = default!;
    }
}
