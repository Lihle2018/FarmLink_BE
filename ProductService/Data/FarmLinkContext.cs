using MongoDB.Driver;
using ProductService.Data.Interfaces;
using ProductService.Models;

namespace ProductService.Data
{
    public class FarmLinkContext : IFarmLinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("ProductsTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("ProductsTableName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("ProductsCollectionName"));
            Categories = database.GetCollection<Category>(configuration.GetValue<string>("CategoriesCollectionName"));
            FarmLinkContextSeed.SeedData(Products,Categories);
        }
        public IMongoCollection<Product> Products { get; }

        public IMongoCollection<Category> Categories { get; }
    }
}
