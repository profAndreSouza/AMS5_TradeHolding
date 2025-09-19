using System.Collections.Concurrent;
using Core;

namespace Infra
{

    public class InMemoryQueue : IQueue
    {
        private readonly ConcurrentQueue<IMessage> _q = new();
        private readonly SemaphoreSlim _signal = new(0);

        public string Name { get; }
        public int Count => _q.Count;

        public InMemoryQueue(string name) => Name = name;

        public Task EnqueueAsync(IMessage message)
        {
            _q.Enqueue(message);
            _signal.Release();
            return Task.CompletedTask;
        }

        public async Task<IMessage?> DequeueAsync(CancellationToken ct)
        {
            await _signal.WaitAsync(ct);
            _q.TryDequeue(out var msg);
            return msg;
        }
    }
}