using MongoDB.Driver;
using ReviewService.Data.Interfaces;
using ReviewService.Models;
using ReviewService.Models.RequestModels;
using ReviewService.Models.ResponseModels;
using ReviewService.Repositories.Interfaces;

namespace ReviewService.Repositories
{
    public class RatingsRepository : IRatingsRepository
    {
        private readonly IFarmLinkContext _context;
        public RatingsRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<RatingResponseModel> AddRatingAsync(RatingRequestModel Request)
        {
            try
            {
                var rating = new Rating(Request);
                await _context.Ratings.InsertOneAsync(rating);
                return new RatingResponseModel(rating);
            }
            catch (Exception e)
            {
                return new RatingResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> DeleteRatingAsync(string ratingId)
        {
            var result =await _context.Ratings.DeleteOneAsync(ratingId);
            return result.DeletedCount;
        }

        public async Task<RatingResponseModel> GetRatingByIdAsync(string ratingId)
        {
            try
            {
                var result = await _context.Ratings.Find(r => r.Id == ratingId).FirstOrDefaultAsync();
                return new RatingResponseModel(result);
            }
            catch (Exception e)
            {
                return new RatingResponseModel(null, e.Message, true);
            }
        }

        public async Task<IEnumerable<RatingResponseModel>> GetRatingsAsync()
        {
            try
            {
                var results = await _context.Ratings.Find(r => true).ToListAsync();
                return results.Select(r => new RatingResponseModel(r));
            }
            catch (Exception e)
            {
                return new[] { new RatingResponseModel(null, e.Message, true) };
            }
        }

        public async Task<IEnumerable<RatingResponseModel>> GetRatingsByUserIdAsync(string userId)
        {
            try
            {
                var result = await _context.Ratings.Find(r => r.CreatingUser == userId).ToListAsync();
                return result.Select(r => new RatingResponseModel(r));
            }
            catch (Exception e)
            {
                return new[] { new RatingResponseModel(null, e.Message, true) };
            }
        }

        public async Task<IEnumerable<RatingResponseModel>> GetRatingsForPostAsync(string postId)
        {
            try
            {
                var results = await _context.Ratings.Find(r => r.ReferenceId == postId).ToListAsync();
                return results.Select(r => new RatingResponseModel(r));
            }
            catch (Exception e)
            {
                return new[] { new RatingResponseModel(null, e.Message, true) };
            }
        }

        public async Task<RatingResponseModel> UpdateRatingAsync(RatingRequestModel rating)
        {
            try
            {
                var filter = Builders<Rating>.Filter.Eq(r => r.Id, rating.Id);
                var update = Builders<Rating>.Update
                    .Set(r => r.rating, rating.rating)
                    .Set(r => r.CreatingUser, rating.CreatingUser)
                    .Set(r => r.CreatedDate, rating.CreatedDate)
                    .Set(r => r.ModifiedDate, DateTime.UtcNow.ToString())
                    .Set(r => r.ModifyingUser, rating.ModifyingUser)
                    .Set(r => r.ReferenceId, rating.ReferenceId)
                    .Set(r => r.Type, rating.Type)
                    .Set(r => r.State, rating.State);

                var options = new FindOneAndUpdateOptions<Rating>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                };

                var result= await _context.Ratings.FindOneAndUpdateAsync(filter, update, options);
                return new RatingResponseModel(result);
            }
            catch (Exception e)
            {
                return new RatingResponseModel(null, e.Message, true);
            }
        }

    }
}
