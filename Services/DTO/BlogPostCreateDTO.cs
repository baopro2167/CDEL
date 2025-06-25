using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class BlogPostCreateDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string Author { get; set; }
       
    }
}
