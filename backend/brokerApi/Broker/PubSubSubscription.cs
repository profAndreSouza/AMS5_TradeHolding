using System.Threading.Channels;

namespace SimpleBroker.Broker;

internal sealed class PubSubSubscription : IDisposable
{
    private readonly Channel<Message> _channel;
    private readonly List<Channel<Message>> _subscribers;
    private readonly Task _worker;

    public PubSubSubscription(Channel<Message> channel, List<Channel<Message>> subscribers, Task worker)
    {
        _channel = channel;
        _subscribers = subscribers;
        _worker = worker;
    }

    public void Dispose()
    {
        _subscribers.Remove(_channel);
        _channel.Writer.Complete();
    }
}
