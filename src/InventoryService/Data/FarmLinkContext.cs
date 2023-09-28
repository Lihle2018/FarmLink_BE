using InventoryService.Data.Interfaces;
using InventoryService.Models;
using MongoDB.Driver;

namespace InventoryService.Data
{
    public class FarmLinkContext : IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("InventoryTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("InventoryTableName"));
            Inventory = database.GetCollection<InventoryItem>(configuration.GetValue<string>("InventoryCollectionName"));
            FarmLinkContextSeed.SeedData(Inventory);
        }
        public IMongoCollection<InventoryItem> Inventory { get; }
    }
}
