using FarmLink.Shared.ResponseModel;

namespace InventoryService.Models.ResponseModels
{
    public class InventoryItemResponseModel:ResponseBase<InventoryItem>
    {
        public InventoryItemResponseModel(InventoryItem item, string message=null)
        {
            Data= item;
            Message= message;
        }
    }
}
