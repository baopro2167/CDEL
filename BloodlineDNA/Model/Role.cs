﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BloodlineDNA.Model

{
	[Table("Role")]
    public class Role
	{
		public int Role { get; set; };
    }
}