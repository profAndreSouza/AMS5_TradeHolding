using Core;

namespace Services
{

    public class Producer
    {
        private readonly IBroker _broker;
        public Producer(IBroker broker) => _broker = broker;

        public Task SendAsync(string exchange, string routingKey, string payload)
        {
            var msg = new Message(routingKey, System.Text.Encoding.UTF8.GetBytes(payload));
            return _broker.PublishAsync(exchange, msg);
        }
    }
}