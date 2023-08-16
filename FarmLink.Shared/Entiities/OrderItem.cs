namespace FarmLink.Shared.Entiities
{
    public sealed class OrderItem
    {
        public string Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
