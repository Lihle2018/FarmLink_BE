using MongoDB.Driver;
using ReviewService.Data.Interfaces;
using ReviewService.Models;
using ReviewService.Models.RequestModels;
using ReviewService.Repositories.Interfaces;
using System.Runtime.CompilerServices;

namespace ReviewService.Repositories
{
    public class RatingsRepository : IRatingsRepository
    {
        private readonly IFarmLinkContext _context;
        public RatingsRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Rating> AddRatingAsync(RatingRequestModel Request)
        {
            var rating = new Rating(Request);
            await _context.Ratings.InsertOneAsync(rating);
            return rating;
        }

        public async Task<long> DeleteRatingAsync(string ratingId)
        {
            var result =await _context.Ratings.DeleteOneAsync(ratingId);
            return result.DeletedCount;
        }

        public async Task<Rating> GetRatingByIdAsync(string ratingId)
        {
            var result = await _context.Ratings.Find(r => r.Id == ratingId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<Rating>> GetRatingsAsync()
        {
           var results =await _context.Ratings.Find(r=>true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(string userId)
        {
           var result =await _context.Ratings.Find(r=>r.CreatingUser== userId).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Rating>> GetRatingsForPostAsync(string postId)
        {
            var results =await _context.Ratings.Find(r=>r.ReferenceId==postId).ToListAsync();
            return results;
        }

        public async Task<Rating> UpdateRatingAsync(RatingRequestModel rating)
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
                ReturnDocument = ReturnDocument.After
            };

            return await _context.Ratings.FindOneAndUpdateAsync(filter, update, options);
        }

    }
}
