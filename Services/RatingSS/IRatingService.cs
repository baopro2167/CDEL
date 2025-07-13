using Model;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RatingSS
{
    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetAllRatingAsync();
        Task<PaginatedList<Rating>> GetAll(int pageNumber, int pageSize);
        Task<Rating?> GetByIdAsync(int id);
        Task<Rating> AddRatingAsync(AddRatingDTO createRatingDto);
        Task<Rating?> UpdateRatingAsync(int id, UpdateRatingDTO updateRatingDto);
        Task DeleteRatingAsync(int id);


        Task<PaginatedList<Rating>> GetByUserIdAsync(int userId, int pageNumber, int pageSize);
        Task<PaginatedList<Rating>> GetByRequestIdAsync(int requestId, int pageNumber, int pageSize);

    }
}
