using CommunicationService.Models;

namespace CommunicationService.Repositories.Interfaces
{
    public interface IMessageChatRepository
    {
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(string chatRoomId, int numberOfMessages);

        Task<ChatMessage> SendMessageAsync(string chatRoomId, string userId, string message);
        
        Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(string chatRoomId, int numberOfMessages);
        Task<long> DeleteMessageAsync(string messageId);
    }
}
