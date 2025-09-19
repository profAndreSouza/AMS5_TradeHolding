namespace Core
{

    public interface IExchange
    {
        string Name { get; }
        ExchangeType Type { get; }
        void BindQueue(string routingKey, IQueue queue); // simple binding by key
        void UnbindQueue(string routingKey, IQueue queue);
        Task RouteAsync(IMessage message);
    }
}