using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FarmLink.Shared.Entiities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentTransactionId { get; set; }
        public string DeliveryNote { get; set; }
        public DeliveryWindow DeliveryWindow { get; set; }
        public string CancellationReason { get; set; }
        public decimal RefundAmount { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
    public enum PaymentMethod 
    {
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }
}
