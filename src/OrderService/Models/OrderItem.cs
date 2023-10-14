namespace FarmLink.OrderService.Models
{
    public sealed class OrderItem
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
