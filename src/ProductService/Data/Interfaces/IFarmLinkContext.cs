using MongoDB.Driver;
using ProductService.Models;

namespace ProductService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Category> Categories { get; }
    }
}
