namespace FarmLink.Shared.Entiities
{
    public class DatabaseSettings
    {
        public string UserCollectionName { get; set; }
        public string ProductCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string SuccessUrl { get; set; }
        public string ErrorUrl { get; set; }
    }
}
