namespace Core
{

    public interface IQueue
    {
        string Name { get; }
        Task EnqueueAsync(IMessage message);
        Task<IMessage?> DequeueAsync(CancellationToken ct);
        int Count { get; }
    }
}