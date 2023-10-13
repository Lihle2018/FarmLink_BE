using FarmLink.OrderService.Models;
using FarmLink.Shared.Enumarations;
using MongoDB.Driver;
using OrderService.Data.Interfaces;
using OrderService.Repositories.Interfaces;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IFarmLinkContext _context;

        public OrderRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<OrderResponseModel> AddOrderAsync(OrderRequestModel Request)
        {
            try
            {
                var order = new Order(Request);
                await _context.Orders.InsertOneAsync(order);
                return new OrderResponseModel(order);
            }
            catch (Exception e)
            {
                return new OrderResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> DeleteOrderAsync(string Id)
        {
            var filter = Builders<Order>.Filter.Eq(c => c.Id, Id);
            var result = await _context.Orders.DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersAsync()
        {
            try
            {
                var orders = await _context.Orders.FindAsync(o => true);
                var result= ExcludeDeletedOrders(orders.ToList());
                return result.Select(o => new OrderResponseModel(o));
            }
            catch (Exception e)
            {
                return new[] { new OrderResponseModel(null, e.Message, true) };
            }
        }

        public async Task<OrderResponseModel> GetOrderAsync(string Id)
        {
            try
            {
                var result = await _context.Orders.FindAsync(o => o.Id == Id && o.State == State.Active);
                return new OrderResponseModel(result.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new OrderResponseModel(null, e.Message, true);
            }
        }

        public async Task<OrderResponseModel> UpdateOrderAsync(OrderRequestModel request)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(o => o.Id, request.Id);
                var update = Builders<Order>.Update
                    .Set(o => o.CustomerId, request.CustomerId)
                    .Set(o => o.Items, request.Items)
                    .Set(o => o.CreatedAt, request.CreatedAt)
                    .Set(o => o.UpdatedAt, DateTime.UtcNow)
                    .Set(o => o.Status, request.Status)
                    .Set(o => o.TotalAmount, request.TotalAmount)
                    .Set(o => o.TaxAmount, request.TaxAmount)
                    .Set(o => o.DeliveryFee, request.DeliveryFee)
                    .Set(o => o.PaymentMethod, request.PaymentMethod)
                    .Set(o => o.PaymentTransactionId, request.PaymentTransactionId)
                    .Set(o => o.DeliveryNote, request.DeliveryNote)
                    .Set(o => o.DeliveryWindow, request.DeliveryWindow)
                    .Set(o => o.CancellationReason, request.CancellationReason)
                    .Set(o => o.RefundAmount, request.RefundAmount)
                    .Set(o => o.VendorId, request.VendorId)
                    .Set(o => o.State, request.State);

                var options = new FindOneAndUpdateOptions<Order>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                };

                var result= await _context.Orders.FindOneAndUpdateAsync(filter, update, options);
                return new OrderResponseModel(result);
            }
            catch (Exception e)
            {
                return new OrderResponseModel(null, e.Message, true);
            }
        }

        public async Task<long> SoftDeleteOrderAsync(string id)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
            var update = Builders<Order>.Update
                .Set(o => o.State, State.Deleted)
                .Set(o => o.UpdatedAt, DateTime.UtcNow);

            var updateResult = await _context.Orders.UpdateOneAsync(filter, update);
            return updateResult.ModifiedCount;
        }
        private IEnumerable<Order> ExcludeDeletedOrders(IEnumerable<Order> orders)
        {
            return orders.Where(o => o.State == State.Active).ToList();
        }
    }
}
