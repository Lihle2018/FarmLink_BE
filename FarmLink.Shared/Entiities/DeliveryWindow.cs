

namespace FarmLink.Shared.Entiities
{
    public sealed class DeliveryWindow
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration => End - Start;
        public bool IsValid => End >= Start;
    }
}
