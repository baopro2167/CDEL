using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model;
using Repositories.BlogPostRepo;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.BlogPostSS
{
    public class BlogPostServices : IBlogPostServices
    {
        private readonly IBlogPostRepository _blogPostRepository;
        public BlogPostServices(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }
      
        public async Task<GetIdBlogPostDTO> GetByIdAsync(int id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);

            if (blogPost == null)
            {
                throw new KeyNotFoundException("Blog post not found.");
            }

            var blogPostDTO = new GetIdBlogPostDTO
            {
                Id = blogPost.Id,
              
                Title = blogPost.Title,
                Content = blogPost.Content,
               
                Author = blogPost.Author,
                CreateAt = blogPost.CreateAt,
                 UpdateAt = blogPost.UpdateAt  
            };

            return blogPostDTO;
        }

   

        public Task<PaginatedList<BlogPost>> GetAll(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogPostResponseDTO> AddAsync(BlogPostCreateDTO blogPostCreateDTO, int userId)
        {
           
            var blogPost = new BlogPost
            {
                Title = blogPostCreateDTO.Title,
                Content = blogPostCreateDTO.Content,
                UserId = userId,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
                Author = blogPostCreateDTO.Author,
                
            };
           
            // Lưu bài viết vào cơ sở dữ liệu
            await _blogPostRepository.AddAsync(blogPost);

            // Trả về đối tượng BlogPostResponseDTO
            return new BlogPostResponseDTO
            {
                BlogId = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
            };
        }
        

        public async Task<BlogPostResponseDTO?> UpdateAsync(int id, BlogPostUpdateDTO blogPostUpdateDTO)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);

            if (blogPost == null)
            {
                throw new Exception("Blog post not found.");
            }

            // Cập nhật thông tin bài viết
            blogPost.Title = blogPostUpdateDTO.Title;
            blogPost.Content = blogPostUpdateDTO.Content;
            blogPost.UpdateAt = DateTime.UtcNow; // Cập nhật thời gian

            // Lưu thay đổi vào cơ sở dữ liệu
            await _blogPostRepository.UpdateAsync(blogPost);

            // Trả về BlogPostResponseDTO
            return new BlogPostResponseDTO
            {
                BlogId = blogPost.Id,
                Title = blogPost.Title
            };
        }
        

        public async Task DeleteAsync(int id)
        {
            var blogPostD = await _blogPostRepository.GetByIdAsync(id);
            if (blogPostD == null)
            {
                throw new KeyNotFoundException($"Examination request with ID {id} not found.");
            }
            await _blogPostRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetAllBlogPostDTO>> GetAllAsync()
        {
            var blogPosts = await _blogPostRepository.GetAll()
                                                     .Select(blog => new GetAllBlogPostDTO
                                                     {
                                                         Id = blog.Id,
                                                         Title = blog.Title,
                                                        
                                                         Author = blog.Author,
                                                         CreateAt = blog.CreateAt
                                                     })
                                                     .ToListAsync();

            return blogPosts;
        }
    }
}
