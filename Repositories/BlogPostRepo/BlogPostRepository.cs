using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.BlogPostRepo
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloodlineDbContext _context;

        public BlogPostRepository(BloodlineDbContext context)
        {
            _context = context;
        }
        public async Task<BlogPost> GetByIdAsync(int id)
        {
            return await _context.Set<BlogPost>().FindAsync(id);
        }
        public IQueryable<BlogPost> GetAll()
        {
            return _context.BlogPosts.AsQueryable();
        }
        public async Task<IEnumerable<BlogPost>> GetAsync()
        {
            return await _context.Set<BlogPost>().ToListAsync();
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            _context.Set<BlogPost>().Add(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }
        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            _context.Entry(blogPost).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return blogPost;
        }
        public async Task DeleteAsync(int id)
        {
            var blogPost = await _context.Set<BlogPost>().FindAsync(id);
            if (blogPost != null)
            {
                _context.Set<BlogPost>().Remove(blogPost);
                await _context.SaveChangesAsync();
            }
        }
       


    }
}
