using Core;

namespace Services
{

    public interface IBroker
    {
        IExchange DeclareExchange(string name, ExchangeType type);
        IQueue DeclareQueue(string name);
        void Bind(string exchangeName, string routingKey, string queueName);
        Task PublishAsync(string exchangeName, IMessage message);
        IQueue? GetQueue(string name);
        IExchange? GetExchange(string name);
        Task MoveToDeadLetterAsync(IMessage msg);
    }

}