using FarmLink.IndentityService.Models;
using IdentityService.Data.Interfaces;
using MongoDB.Driver;

namespace IdentityService.Data
{
    public class FarmLinkContext : IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("UsersTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("UsersTableName"));

            Users = database.GetCollection<User>(configuration.GetValue<string>("UsersCollectionName"));
            FarmLinkContextSeed.SeedData(Users);
        }
        public IMongoCollection<User> Users { get; }
    }
}
