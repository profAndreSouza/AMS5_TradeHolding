using System.Collections.Concurrent;
using Core;
using Infra;

namespace Services
{

    public class Broker : IBroker
    {
        private readonly ConcurrentDictionary<string, IExchange> _exchanges = new();
        private readonly ConcurrentDictionary<string, IQueue> _queues = new();

        // For dead-lettering control
        public string DeadLetterQueueName { get; } = "DLQ";

        public Broker()
        {
            // create default DLQ
            DeclareQueue(DeadLetterQueueName);
        }

        public IExchange DeclareExchange(string name, ExchangeType type)
        {
            return _exchanges.GetOrAdd(name, _ => new InMemoryExchange(name, type));
        }

        public IQueue DeclareQueue(string name)
        {
            return _queues.GetOrAdd(name, _ => new InMemoryQueue(name));
        }

        public void Bind(string exchangeName, string routingKey, string queueName)
        {
            var ex = DeclareExchange(exchangeName, ExchangeType.Direct);
            var q = DeclareQueue(queueName);
            ex.BindQueue(routingKey, q);
        }

        public async Task PublishAsync(string exchangeName, IMessage message)
        {
            if (!_exchanges.TryGetValue(exchangeName, out var ex))
                throw new InvalidOperationException($"Exchange {exchangeName} not found");

            await ex.RouteAsync(message);
        }

        public IQueue? GetQueue(string name) => _queues.TryGetValue(name, out var q) ? q : null;
        public IExchange? GetExchange(string name) => _exchanges.TryGetValue(name, out var ex) ? ex : null;

        // helper to move to DLQ
        public Task MoveToDeadLetterAsync(IMessage msg)
        {
            var dlq = GetQueue(DeadLetterQueueName)!;
            return dlq.EnqueueAsync(msg);
        }
    }
}