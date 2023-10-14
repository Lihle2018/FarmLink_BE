using CustomerService.Data.Interfaces;
using FarmLink.CustomerService.Models;
using MongoDB.Driver;

namespace CustomerService.Data
{
    public class FarmLinkContext : IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("CustomersTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("CustomersTableName"));

            Customers = database.GetCollection<Customer>(configuration.GetValue<string>("CustomersCollectionName"));
            FarmLinkContextSeed.SeedData(Customers);
        }
        public IMongoCollection<Customer> Customers { get; }
    }
}
