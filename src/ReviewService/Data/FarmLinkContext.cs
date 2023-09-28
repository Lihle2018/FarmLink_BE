using MongoDB.Driver;
using ReviewService.Data.Interfaces;
using ReviewService.Models;

namespace ReviewService.Data
{
    public class FarmLinkContext : IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {

            var client = new MongoClient(configuration.GetValue<string>("ReviewsTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("ReviewsTableName"));

            Comments = database.GetCollection<Comment>(configuration.GetValue<string>("CommentsCollectionName"));
            Ratings = database.GetCollection<Rating>(configuration.GetValue<string>("RatingsCollectionName"));
            FarmLinkContextSeed.SeedData(Comments, Ratings);
        }
        public IMongoCollection<Comment> Comments { get; }

        public IMongoCollection<Rating> Ratings { get; }

    }
}
