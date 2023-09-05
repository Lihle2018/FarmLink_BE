using CommunicationService.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CommunicationService.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageChatRepository _chatRepository;

        public ChatHub(IMessageChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task SendMessage(string chatRoomId, string userId, string message)
        {
            // Persist the message to the repository
            await _chatRepository.SendMessageAsync(chatRoomId, userId, message);

            // Broadcast the message to all clients in the same chat room
            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", userId, message);
        }

        public async Task JoinChat(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);

            // Retrieve chat history and send it to the joining user
            var chatHistory = await _chatRepository.GetChatHistoryAsync(chatRoomId, numberOfMessages: 10);

            await Clients.Client(Context.ConnectionId).SendAsync("LoadChatHistory", chatHistory);
        }

        public async Task LeaveChat(string chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        }
    }
}
