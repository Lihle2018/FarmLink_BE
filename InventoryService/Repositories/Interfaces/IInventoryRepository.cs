using InventoryService.Models;
using InventoryService.Models.RequestModels;

namespace InventoryService.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync();
        Task<InventoryItem> GetInventoryItemByIdAsync(string id);
        Task<IEnumerable<InventoryItem>> GetInventoryItemsByProductIdAsync(string productId);
        Task<InventoryItem> AddInventoryItemAsync(InventoryItemRequestModel inventoryItem);
        Task<InventoryItem> UpdateInventoryItemAsync(InventoryItemRequestModel inventoryItem);
        Task<long> DeleteInventoryItemAsync(string id);
    }

}
