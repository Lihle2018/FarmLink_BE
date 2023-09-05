using FarmLink.OrderService.Models;

namespace OrderService.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(OrderRequestModel Request);
        Task<Order> UpdateOrderAsync(OrderRequestModel Request);
        Task<long> DeleteOrderAsync(string Id);
        Task<long> SoftDeleteOrderAsync(string Id);
        Task<Order> GetOrderByAsync(string Id);
        Task<IEnumerable<Order>> GetOrdersAsync();
    }
}
