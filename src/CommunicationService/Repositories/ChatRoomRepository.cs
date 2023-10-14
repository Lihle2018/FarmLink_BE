using CommunicationService.Data.Interfaces;
using CommunicationService.Models;
using CommunicationService.Repositories.Interfaces;
using MongoDB.Driver;

namespace CommunicationService.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly IFarmlinkContext _context;
        public ChatRoomRepository(IFarmlinkContext context)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ChatRoom> CreateChatRoomAsync(string user1Id, string user2Id,string name=null)
        {
            var users= new List<string>() { user1Id, user2Id };
            var chatroom = new ChatRoom()
            {
                UserIds = users,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user1Id,
                Name = name
            };
            await _context.ChatRooms.InsertOneAsync(chatroom);
            return chatroom;
        }

        public async Task<long> DeleteChatRoomAsync(string chatRoomId)
        {
           var result = await _context.ChatRooms.DeleteOneAsync(chatRoomId);
            return result.DeletedCount;
        }

        public async Task<ChatRoom> GetChatRoomByIdAsync(string chatRoomId)
        {
            var result = await _context.ChatRooms.Find(c => c.Id == chatRoomId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(string userId)
        {
            var result = await _context.ChatRooms.Find(c => c.UserIds.Contains(userId)).ToListAsync();
            return result;
        }

        public async Task<ChatRoom> UpdateChatRoomAsync(ChatRoom chatRoom)
        {
            var filter = Builders<ChatRoom>.Filter.Eq(x => x.Id, chatRoom.Id);
            var update = Builders<ChatRoom>.Update
                .Set(x => x.Name, chatRoom.Name)
                .Set(x => x.UserIds, chatRoom.UserIds)
                .Set(x => x.CreatedAt, chatRoom.CreatedAt)
                .Set(x => x.CreatedBy, chatRoom.CreatedBy)
                .Set(x => x.LastUpdatedAt, chatRoom.LastUpdatedAt)
                .Set(x => x.LastUpdatedBy, chatRoom.LastUpdatedBy);

            var options = new FindOneAndUpdateOptions<ChatRoom>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = false // Do not insert a new document if not found
                
            };

            var updatedChatRoom = await _context.ChatRooms.FindOneAndUpdateAsync(filter, update, options);

            return updatedChatRoom;
        }
    }
}
