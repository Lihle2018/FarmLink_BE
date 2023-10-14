using FarmLink.OrderService.Models;
using MongoDB.Driver;

namespace OrderService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        IMongoCollection<Order> Orders { get; }
    }
}
