using Model;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BlogPostSS
{
    public interface IBlogPostServices
    {
        Task<IEnumerable<GetAllBlogPostDTO>> GetAllAsync();
        Task<GetIdBlogPostDTO?> GetByIdAsync(int id);
        Task<PaginatedList<BlogPost>> GetAll(int pageNumber, int pageSize);
        
        Task<BlogPostResponseDTO> AddAsync(BlogPostCreateDTO blogPostCreateDTO, int userId);
        Task<BlogPostResponseDTO?> UpdateAsync(int id, BlogPostUpdateDTO blogPostUpdateDTO);
        Task DeleteAsync(int id);
    }
}
