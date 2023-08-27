using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FarmLink.Shared.RequestModels;
using FarmLink.Shared.Enumarations;

namespace FarmLink.OrderService.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
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
        public string VendorId { get; set; }
        public State State { get; set; }
        public Order() { }
        public Order(OrderRequestModel Request)
        {
            Id= Request.Id;
            CustomerId= Request.CustomerId;
            Items= Request.Items;
            CreatedAt = Request.CreatedAt;
            UpdatedAt = Request.UpdatedAt;
            Status = Request.Status;
            TotalAmount = Request.TotalAmount;
            TaxAmount = Request.TaxAmount;
            DeliveryFee= Request.DeliveryFee;
            PaymentMethod= Request.PaymentMethod;
            PaymentTransactionId= Request.PaymentTransactionId;
            DeliveryNote= Request.DeliveryNote;
            DeliveryWindow= Request.DeliveryWindow;
            CancellationReason= Request.CancellationReason;
            RefundAmount= Request.RefundAmount;
            VendorId= Request.VendorId;
            State= Request.State;
        }
    }
    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Cash,
        MobilePayment
    }
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
