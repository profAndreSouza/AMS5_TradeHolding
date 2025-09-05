using System.Threading.Channels;

namespace SimpleBroker.Broker;

internal sealed class InMemoryQueue
{
    public string Name { get; }
    private readonly Channel<Message> _channel;
    private readonly Channel<Message> _deadLetter;

    public InMemoryQueue(string name, int capacity = 10_000)
    {
        Name = name;
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };
        _channel = Channel.CreateBounded<Message>(options);
        _deadLetter = Channel.CreateUnbounded<Message>();
    }

    public ValueTask WriteAsync(Message message, CancellationToken ct) => _channel.Writer.WriteAsync(message, ct);
    public IAsyncEnumerable<Message> ReadAllAsync(CancellationToken ct) => _channel.Reader.ReadAllAsync(ct);

    public ValueTask WriteDeadLetterAsync(Message message, CancellationToken ct) => _deadLetter.Writer.WriteAsync(message, ct);
    public IAsyncEnumerable<Message> ReadAllDeadLettersAsync(CancellationToken ct) => _deadLetter.Reader.ReadAllAsync(ct);
}
