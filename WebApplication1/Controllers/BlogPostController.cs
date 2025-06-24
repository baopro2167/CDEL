using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.BlogPostSS;
using Services.DTO;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [Route("api/BlogPost")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostServices _blogPostServices;
        public BlogPostController(IBlogPostServices blogPostServices)
        {
            _blogPostServices = blogPostServices;
        }
        /// <summary>
        /// GET: blogpost theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetIdBlogPostDTO>> GetBlogPostById(int id)
        {
            var blogPost = await _blogPostServices.GetByIdAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);  // Trả về BlogPostDTO mà không có trường UpdateAt
        }
        /// <summary>
        /// GET: toàn bộ blogpost
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllBlogPostDTO>>> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostServices.GetAllAsync();
            return Ok(blogPosts); // Trả về danh sách BlogPostDTO
        }



        /// <summary>
        /// cap nhật blogpost
        /// </summary>
        [Authorize(Roles = "1,4")]
        [HttpPut("{blogId}")]
       
        public async Task<ActionResult<BlogPostResponseDTO>> UpdateBlogPost(int blogId, [FromBody] BlogPostUpdateDTO blogPostUpdateDTO)
        {
            if (blogPostUpdateDTO == null)
            {
                return BadRequest("Invalid blog post data.");
            }

            try
            {
                // Cập nhật bài viết qua service
                var response = await _blogPostServices.UpdateAsync(blogId, blogPostUpdateDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [Authorize(Roles = "1,4")]
        [HttpPost]
        public async Task<ActionResult<BlogPostResponseDTO>> CreateBlogPost([FromBody] BlogPostCreateDTO blogPostCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Lấy UserId từ Claims (NameIdentifier)
            var userIdClaim = User.Claims
                                  .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Không xác thực được UserId.");

            // Gọi service
            var result = await _blogPostServices.AddAsync(blogPostCreateDTO, userId);
            return Ok(result);
        }
        /// <summary>
        /// Xóa BlogPost
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var BlogpostDD = await _blogPostServices.GetByIdAsync(id);
                if (BlogpostDD == null)
                {
                    return NotFound($"Kit with ID {id} not found.");
                }

                await _blogPostServices.DeleteAsync(id);
                return Ok("Xóa thành công"); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting blogpost {id}: {ex.Message}");
                return NotFound(ex.Message);
            }

        }
    }
}
