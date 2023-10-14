using ReviewService.Models.RequestModels;
using ReviewService.Models.ResponseModels;

namespace ReviewService.Repositories.Interfaces
{
    public interface IRatingsRepository
    {
        Task<IEnumerable<RatingResponseModel>> GetRatingsAsync();
        Task<RatingResponseModel> GetRatingByIdAsync(string ratingId);
        Task<IEnumerable<RatingResponseModel>> GetRatingsByUserIdAsync(string userId);
        Task<IEnumerable<RatingResponseModel>> GetRatingsForPostAsync(string productId);
        Task<RatingResponseModel> AddRatingAsync(RatingRequestModel rating);
        Task<RatingResponseModel> UpdateRatingAsync(RatingRequestModel rating);
        Task<long> DeleteRatingAsync(string ratingId);
    }
}
