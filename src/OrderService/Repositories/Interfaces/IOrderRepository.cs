using FarmLink.OrderService.Models;

namespace OrderService.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderResponseModel> AddOrderAsync(OrderRequestModel Request);
        Task<OrderResponseModel> UpdateOrderAsync(OrderRequestModel Request);
        Task<long> DeleteOrderAsync(string Id);
        Task<long> SoftDeleteOrderAsync(string Id);
        Task<OrderResponseModel> GetOrderByAsync(string Id);
        Task<IEnumerable<OrderResponseModel>> GetOrdersAsync();
    }
}
