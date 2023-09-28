using FarmLink.OrderService.Models;
using MongoDB.Driver;
using OrderService.Data.Interfaces;

namespace OrderService.Data
{
    public class FarmLinkContext : IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("OrdersTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("OrdersTableName"));

            Orders = database.GetCollection<Order>(configuration.GetValue<string>("OrdersCollectionName"));
            FarmLinkContextSeed.SeedData(Orders);
        }
        public IMongoCollection<Order> Orders { get; }
    }
}
