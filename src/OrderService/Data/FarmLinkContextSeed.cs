using FarmLink.OrderService.Models;
using FarmLink.Shared.Enumarations;
using MongoDB.Driver;

namespace OrderService.Data
{
    public class FarmLinkContextSeed
    {
        public static void SeedData(IMongoCollection<Order> Collection)
        {
            bool existOrder = Collection.Find(p => true).Any();
            if (!existOrder)
                Collection.InsertManyAsync(GetPreconfiguredOrders());
        }
        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
    {
        new Order
        {
            CustomerId = "customer123",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = "customer123",
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 2,
                    UnitPrice = 10.99M
                },
                new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = "customer123",
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 3,
                    UnitPrice = 5.99M
                }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = OrderStatus.Processing,
            TotalAmount = 31.95M,
            TaxAmount = 3.00M,
            DeliveryFee = 7.50M,
            PaymentMethod = PaymentMethod.CreditCard,
            PaymentTransactionId = Guid.NewGuid().ToString(),
            DeliveryNote = "Fragile items",
            DeliveryWindow = new DeliveryWindow
            {
                Id = "window1",
                Start = DateTime.UtcNow.AddHours(2),
                End = DateTime.UtcNow.AddHours(4)
            },
            CancellationReason = "",
            RefundAmount = 0.00M,
            VendorId = "Vend1",
            State = State.Active
        },
        new Order
        {
            CustomerId = "customer456",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = "customer456",
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    UnitPrice = 15.50M
                }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = OrderStatus.Delivered,
            TotalAmount = 15.50M,
            TaxAmount = 1.50M,
            DeliveryFee = 5.00M,
            PaymentMethod = PaymentMethod.Cash,
            PaymentTransactionId = Guid.NewGuid().ToString(),
            DeliveryNote = "Leave at doorstep",
            DeliveryWindow = new DeliveryWindow
            {
                Id = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow.AddHours(3),
                End = DateTime.UtcNow.AddHours(5)
            },
            CancellationReason = "",
            RefundAmount = 0.00M,
            VendorId = "Vend1",
            State = State.Active
        },
        new Order
        {
            CustomerId = "customer789",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId ="customer789" ,
                    ProductId = "Prod1",
                    Quantity = 4,
                    UnitPrice = 8.75M
                }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalAmount = 35.00M,
            TaxAmount = 2.50M,
            DeliveryFee = 8.00M,
            PaymentMethod = PaymentMethod.BankTransfer,
            PaymentTransactionId = Guid.NewGuid().ToString(),
            DeliveryNote = "Call before delivery",
            DeliveryWindow = new DeliveryWindow
            {
                Id = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow.AddHours(4),
                End = DateTime.UtcNow.AddHours(6)
            },
            CancellationReason = "",
            RefundAmount = 0.00M,
            VendorId = "Vend1",
            State = State.Active
        }
    };
        }

    }
}
