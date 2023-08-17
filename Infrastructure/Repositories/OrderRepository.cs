using FarmLink.Shared.Entiities;
using FarmLink.Shared.RequestModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        bool Connect();
        Task<Order> AddOrderAsync(OrderRequestModel Request);
        Task<Order> UpdateOrderAsync(OrderRequestModel Request);
        Task<long> DeleteOrderAsync(string Id);
        Task<long> SoftDeleteOrderAsync(string Id);
        Task<Order> GetOrderByIdAsync(string Id);
        Task<List<Order>> GetAllOrders();
    }
    public class OrderRepository : IOrderRepository
    {
        IMongoCollection<Order> _orders;
        private readonly IConfiguration _configuration;

        private readonly IOptions<DatabaseSettings> _config;

        public OrderRepository(IOptions<DatabaseSettings> config, IConfiguration configuration)
        {
            _config = config;

            _configuration = configuration;
        }

        public bool Connect()
        {
            try
            {
                var t = _config.Value;
                var client = new MongoClient(_configuration.GetSection("OrdersTableConnection").Value);
                var database = client.GetDatabase(_configuration.GetSection("OrdersTableName").Value);

                _orders = database.GetCollection<Order>(_configuration.GetSection("OrdersCollectionName").Value);
                return true;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Order> AddOrderAsync(OrderRequestModel Request)
        {
            Connect();
            var order = new Order(Request);
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<long> DeleteOrderAsync(string Id)
        {
            Connect();
            var filter = Builders<Order>.Filter.Eq(c => c.Id, Id);
            var result = await _orders.DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            Connect();
            var orders = await _orders.FindAsync(o => true);
            return ExcludeDeletedOrders(orders.ToList());
        }

        public async Task<Order> GetOrderByIdAsync(string Id)
        {
            Connect();
            var result=await _orders.FindAsync(o=>o.Id == Id&&o.State==State.Active);
            return result.FirstOrDefault();
        }

        public async Task<Order> UpdateOrderAsync(OrderRequestModel request)
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
                ReturnDocument = ReturnDocument.After
            };

            return await _orders.FindOneAndUpdateAsync(filter, update, options);
        }

        public async Task<long> SoftDeleteOrderAsync(string id)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
            var update = Builders<Order>.Update
                .Set(o => o.State, State.Deleted)
                .Set(o => o.UpdatedAt, DateTime.UtcNow);

            var updateResult = await _orders.UpdateOneAsync(filter, update);
            return updateResult.ModifiedCount;
        }
        private List<Order> ExcludeDeletedOrders(List<Order> orders)
        {
            return orders.Where(o=>o.State==State.Active).ToList();
        }
    }
}
