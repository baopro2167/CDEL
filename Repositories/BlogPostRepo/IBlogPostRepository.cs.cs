using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.BlogPostRepo
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAsync();
        Task<BlogPost> GetByIdAsync(int id);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost> UpdateAsync(BlogPost blogPost);
        Task DeleteAsync(int id);
        IQueryable<BlogPost> GetAll();

    }
}
