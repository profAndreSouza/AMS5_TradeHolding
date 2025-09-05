using System.Collections.Concurrent;
using System.Threading.Channels;

namespace SimpleBroker.Broker;

public sealed class SimpleBroker
{
    private readonly ConcurrentDictionary<string, InMemoryQueue> _queues = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, List<Channel<Message>>> _topics = new(StringComparer.OrdinalIgnoreCase);

    // ---------- Fila (Work Queue) ----------
    public void DeclareQueue(string name, int capacity = 10_000) =>
        _queues.GetOrAdd(name, n => new InMemoryQueue(n, capacity));

    public async Task PublishToQueueAsync(string queueName, string body, CancellationToken ct = default)
    {
        if (!_queues.TryGetValue(queueName, out var q))
            throw new InvalidOperationException($"Queue '{queueName}' não existe.");
        await q.WriteAsync(Message.Create(body), ct);
    }

    public IDisposable SubscribeQueue(
        string queueName,
        Func<Message, Task<bool>> handler,
        ConsumerOptions? options = null,
        CancellationToken externalToken = default)
    {
        if (!_queues.TryGetValue(queueName, out var q))
            throw new InvalidOperationException($"Queue '{queueName}' não existe.");

        options ??= new ConsumerOptions();
        var cts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
        var token = cts.Token;

        var workers = new List<Task>();
        for (int i = 0; i < options.DegreeOfParallelism; i++)
        {
            workers.Add(Task.Run(async () =>
            {
                await foreach (var msg in q.ReadAllAsync(token))
                {
                    var processed = false;
                    var attemptMsg = msg;

                    for (int attempt = msg.DeliveryCount + 1; attempt <= options.MaxRetries + 1; attempt++)
                    {
                        try
                        {
                            processed = await handler(attemptMsg);
                        }
                        catch { processed = false; }

                        if (processed) break;
                        else
                        {
                            if (attempt > options.MaxRetries)
                            {
                                await q.WriteDeadLetterAsync(attemptMsg.WithIncrementedDelivery(), token);
                                break;
                            }
                            else
                            {
                                attemptMsg = attemptMsg.WithIncrementedDelivery();
                                await q.WriteAsync(attemptMsg, token);
                                break;
                            }
                        }
                    }
                }
            }, token));
        }

        return new Subscription(cts, workers);
    }

    public IAsyncEnumerable<Message> DeadLetters(string queueName, CancellationToken ct = default)
    {
        if (!_queues.TryGetValue(queueName, out var q))
            throw new InvalidOperationException($"Queue '{queueName}' não existe.");
        return q.ReadAllDeadLettersAsync(ct);
    }

    // ---------- Pub/Sub ----------
    public void DeclareTopic(string name) =>
        _topics.TryAdd(name, new List<Channel<Message>>());

    public void PublishToTopic(string topic, string body)
    {
        if (!_topics.TryGetValue(topic, out var subs)) return;
        var msg = Message.Create(body);
        foreach (var channel in subs)
            channel.Writer.TryWrite(msg);
    }

    public IDisposable SubscribeTopic(string topic, Func<Message, Task> handler, CancellationToken token)
    {
        if (!_topics.TryGetValue(topic, out var subs))
            throw new InvalidOperationException($"Topic '{topic}' não existe.");

        var channel = Channel.CreateUnbounded<Message>();
        subs.Add(channel);

        var task = Task.Run(async () =>
        {
            await foreach (var msg in channel.Reader.ReadAllAsync(token))
                await handler(msg);
        }, token);

        return new PubSubSubscription(channel, subs, task);
    }
}
