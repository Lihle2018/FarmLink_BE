using ReviewService.Models;
using ReviewService.Models.RequestModels;

namespace ReviewService.Repositories.Interfaces
{
    public interface IRatingsRepository
    {
        Task<IEnumerable<Rating>> GetRatingsAsync();
        Task<Rating> GetRatingByIdAsync(string ratingId);
        Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(string userId);
        Task<IEnumerable<Rating>> GetRatingsForPostAsync(string productId);
        Task<Rating> AddRatingAsync(RatingRequestModel rating);
        Task<Rating> UpdateRatingAsync(RatingRequestModel rating);
        Task<long> DeleteRatingAsync(string ratingId);
    }
}
