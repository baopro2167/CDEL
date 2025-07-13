using Model;
using Repositories.ExRequestRepo;
using Repositories.Pagging;
using Repositories.RatingRepo;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RatingSS
{
   public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IExRequestRepository _exRequestRepository;

        public RatingService(IRatingRepository ratingRepository, IExRequestRepository exRequestRepository)
        {
            _ratingRepository = ratingRepository;
            _exRequestRepository = exRequestRepository;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingAsync()
        {
            return await _ratingRepository.GetAllAsync();
        }

        public async Task<Rating?> GetByIdAsync(int id)
        {
            return await _ratingRepository.GetByIdAsync(id);
        }

        public async Task<Rating> AddRatingAsync(AddRatingDTO createRatingDto)
        {
            var examinationRequest = await _exRequestRepository.GetByIdAsync(createRatingDto.RequestId);
            if (examinationRequest == null)
                throw new KeyNotFoundException($"ExaminationRequest with ID {createRatingDto.RequestId} not found.");
            var rating = new Rating
            {
                UserId = examinationRequest.UserId,
                RequestId = createRatingDto.RequestId,
                RatingValue = createRatingDto.RatingValue,
                Feedback = createRatingDto.Feedback,
                CreateAt = DateTime.UtcNow
            };
            return await _ratingRepository.AddAsync(rating);
        }

        public async Task<Rating?> UpdateRatingAsync(int id, UpdateRatingDTO updateRatingDto)
        {
            var rating = await _ratingRepository.GetByIdAsync(id);
            if (rating == null)
                throw new KeyNotFoundException($"Rating with ID {id} not found.");

            rating.RatingValue = updateRatingDto.RatingValue;
            rating.Feedback = updateRatingDto.Feedback;
            rating.CreateAt = DateTime.UtcNow; // Cập nhật thời gian nếu cần

            return await _ratingRepository.UpdateAsync(rating);
        }

        public async Task DeleteRatingAsync(int id)
        {
            await _ratingRepository.DeleteAsync(id);
        }

        public async Task<PaginatedList<Rating>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Rating> rating = _ratingRepository.GetAll().AsQueryable();
            return await PaginatedList<Rating>.CreateAsync(rating, pageNumber, pageSize);
        }
        public async Task<PaginatedList<Rating>> GetByUserIdAsync(int userId, int pageNumber, int pageSize)
        {
            var query = _ratingRepository.GetAll().Where(r => r.UserId == userId);
            return await PaginatedList<Rating>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PaginatedList<Rating>> GetByRequestIdAsync(int requestId, int pageNumber, int pageSize)
        {
            var query = _ratingRepository.GetAll().Where(r => r.RequestId == requestId);
            return await PaginatedList<Rating>.CreateAsync(query, pageNumber, pageSize);
        }


    }
}
