using FarmLink.Shared.ResponseModel;

namespace FarmLink.OrderService.Models
{
    public class OrderResponseModel:ResponseBase<Order>
    {
        public OrderResponseModel(Order order,string message=null,bool error=false) 
        { 
            Data = order;
            Message = message;
            Error = error;
        }
    }
}
