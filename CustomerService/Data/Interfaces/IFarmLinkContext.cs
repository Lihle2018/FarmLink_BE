using FarmLink.CustomerService.Models;
using MongoDB.Driver;

namespace CustomerService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        IMongoCollection<Customer> Customers { get; }
    }

}
