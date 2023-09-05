using CommunicationService.Data.Interfaces;
using CommunicationService.Models;
using CommunicationService.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CommunicationService.Repositories
{
    public class MessageChatRepository : IMessageChatRepository
    {
        private readonly IFarmlinkContext _context;
        public MessageChatRepository(IFarmlinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(string chatRoomId, int numberOfMessages)
        {
            var result = await _context.ChatMessages
                                    .Find(c => c.ChatRoomId == chatRoomId)
                                    .Limit(numberOfMessages)
                                    .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(string chatRoomId, int numberOfMessages)
        {
            var result = await _context.ChatMessages
                                    .Find(c => c.ChatRoomId == chatRoomId)
                                    .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(string userId)
        {
            var results = await _context.ChatRooms.Find(c => c.UserIds.Contains(userId)).ToListAsync();
            return results;
        }

        public async Task<ChatMessage> SendMessageAsync(string chatRoomId, string userId, string message)
        {
            var chat = new ChatMessage()
            {
                ChatRoomId = chatRoomId,
                Message = message,
                User = userId,
                Timestamp = DateTime.UtcNow
            };
            await _context.ChatMessages.InsertOneAsync(chat);
            return chat;
        }

        public async Task<long> DeleteMessageAsync(string messageId)
        {
            var result = await _context.ChatMessages.DeleteOneAsync(messageId);
            return result.DeletedCount;
        }
    }
}
