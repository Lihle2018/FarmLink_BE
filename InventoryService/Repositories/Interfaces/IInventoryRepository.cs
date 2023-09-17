using InventoryService.Models;
using InventoryService.Models.RequestModels;
using InventoryService.Models.ResponseModels;

namespace InventoryService.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItemResponseModel>> GetInventoryItemsAsync();
        Task<InventoryItemResponseModel> GetInventoryItemByIdAsync(string id);
        Task<IEnumerable<InventoryItemResponseModel>> GetInventoryItemsByProductIdAsync(string productId);
        Task<InventoryItemResponseModel> AddInventoryItemAsync(InventoryItemRequestModel inventoryItem);
        Task<InventoryItemResponseModel> UpdateInventoryItemAsync(InventoryItemRequestModel inventoryItem);
        Task<long> DeleteInventoryItemAsync(string id);
    }

}
