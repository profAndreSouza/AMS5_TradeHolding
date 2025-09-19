using System.Collections.Concurrent;
using Core;

namespace Infra
{

    public class InMemoryExchange : IExchange
    {
        public string Name { get; }
        public ExchangeType Type { get; }

        // routingKey -> list of queues
        private readonly ConcurrentDictionary<string, ConcurrentBag<IQueue>> _bindings = new();

        public InMemoryExchange(string name, ExchangeType type)
        {
            Name = name;
            Type = type;
        }

        public void BindQueue(string routingKey, IQueue queue)
        {
            var bag = _bindings.GetOrAdd(routingKey ?? string.Empty, _ => new ConcurrentBag<IQueue>());
            bag.Add(queue);
        }

        public void UnbindQueue(string routingKey, IQueue queue)
        {
            // simple approach: no removal from bag (didactic). Could store ConcurrentDictionary<string,List<IQueue>> for removals.
        }

        public async Task RouteAsync(IMessage message)
        {
            if (Type == ExchangeType.Fanout)
            {
                // deliver to all queues across all bindings
                foreach (var kv in _bindings)
                {
                    foreach (var q in kv.Value)
                        await q.EnqueueAsync(message);
                }
                return;
            }

            if (Type == ExchangeType.Direct)
            {
                if (_bindings.TryGetValue(message.RoutingKey, out var bag))
                {
                    foreach (var q in bag)
                        await q.EnqueueAsync(message);
                }
                return;
            }

            if (Type == ExchangeType.Topic)
            {
                // naive topic matching: support * and # can be implemented; for didactic keep direct match
                if (_bindings.TryGetValue(message.RoutingKey, out var bag2))
                {
                    foreach (var q in bag2)
                        await q.EnqueueAsync(message);
                }
                return;
            }
        }
    }
}