using FarmLink.Shared.ResponseModel;

namespace FarmLink.OrderService.Models
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
