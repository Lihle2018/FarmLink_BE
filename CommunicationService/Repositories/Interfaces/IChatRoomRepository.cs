using CommunicationService.Models;

namespace CommunicationService.Repositories.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<ChatRoom> CreateChatRoomAsync(string user1Id, string user2Id, string name = null);

        Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(string userId);

        Task<ChatRoom> GetChatRoomByIdAsync(string chatRoomId);

        Task<ChatRoom> UpdateChatRoomAsync(ChatRoom chatRoom);

        Task<long> DeleteChatRoomAsync(string chatRoomId);
    }

}
