using InventoryService.Models;
using MongoDB.Driver;

namespace InventoryService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        IMongoCollection<InventoryItem> Inventory { get; }
    }

}
