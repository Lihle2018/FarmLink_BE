using CommunicationService.Data.Interfaces;
using CommunicationService.Models;
using MongoDB.Driver;

namespace CommunicationService.Data
{
    public class FarmLinkContext:IFarmlinkContext
    {
        public FarmLinkContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("ChatMessagesTableConnection"));
            var database = client.GetDatabase(configuration.GetValue<string>("ChatMessagesTableName"));

            ChatMessages = database.GetCollection<ChatMessage>(configuration.GetValue<string>("ChatMessagesCollectionName"));
            //FarmLinkContextSeed.SeedData(Customers);
        }

        public IMongoCollection<ChatMessage> ChatMessages { get; }

        public IMongoCollection<ChatRoom> ChatRooms { get; }
    }
}
