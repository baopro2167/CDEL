using Model;
using Repositories.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RatingRepo
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllAsync();
        IQueryable<Rating> GetAll();
        Task<Rating?> GetByIdAsync(int id);
        Task<Rating> AddAsync(Rating rating);
        Task<Rating?> UpdateAsync(Rating rating);
        Task DeleteAsync(int id);
       
    }
}
