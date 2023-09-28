using CommunicationService.Models;
using MongoDB.Driver;

namespace CommunicationService.Data.Interfaces
{
    public interface IFarmlinkContext
    {
        IMongoCollection<ChatMessage> ChatMessages { get; }
        IMongoCollection<ChatRoom> ChatRooms { get; }
    }
}
