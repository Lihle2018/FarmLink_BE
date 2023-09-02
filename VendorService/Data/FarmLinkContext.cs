using DnsClient;
using MongoDB.Driver;
using System.Xml.Linq;
using VendorService.Data.Interfaces;
using VendorService.Models;

namespace VendorService.Data
{
    public class FarmLinkContext:IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {

            var client = new MongoClient(configuration.GetValue<string>("VendorsTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("VendorsTableName"));

            Vendors = database.GetCollection<Vendor>(configuration.GetValue<string>("VendorsCollectionName"));
            FarmLinkContextSeed.SeedData(Vendors);
        }

        public IMongoCollection<Vendor> Vendors { get; }
    }
}
