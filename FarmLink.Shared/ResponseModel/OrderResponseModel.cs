using FarmLink.Shared.Entiities;

namespace FarmLink.Shared.ResponseModel
{
    public class OrderResponseModel:ResponseBase<Order>
    {
        public OrderResponseModel(Order order,string message=null) 
        { 
            Data = order;
            Message = message;
        }
    }
}
