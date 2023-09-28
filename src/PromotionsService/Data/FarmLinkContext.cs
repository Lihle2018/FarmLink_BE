using MongoDB.Driver;
using PromotionsService.Data.Interfaces;
using PromotionsService.Models;

namespace PromotionsService.Data
{
    public class FarmLinkContext : IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("PromotionsTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("PromtionsTableName"));

            Promotions = database.GetCollection<Promotion>(configuration.GetValue<string>("PromotionsCollectionName"));
            FarmLinkContextSeed.SeedData(Promotions);
        }
        public IMongoCollection<Promotion> Promotions { get; }
    }
}
