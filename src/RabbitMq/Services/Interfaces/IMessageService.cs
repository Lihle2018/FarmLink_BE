
namespace RabbitMq.Services.Interfaces
{
    public interface IMessageService
    { 
        bool Enqueue(string message);
    }
}
