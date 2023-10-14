using FarmLink.Shared.ResponseModel;

namespace InventoryService.Models.ResponseModels
{
    public class InventoryItemResponseModel:ResponseBase<InventoryItem>
    {
        public InventoryItemResponseModel(InventoryItem item, string message=null,bool error =false)
        {
            Data= item;
            Message= message;
            Error= error;
        }
    }
}
