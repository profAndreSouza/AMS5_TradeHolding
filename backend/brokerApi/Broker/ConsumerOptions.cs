namespace SimpleBroker.Broker;

public sealed class ConsumerOptions
{
    public int MaxRetries { get; init; } = 5;
    public int Prefetch { get; init; } = 32;
    public int DegreeOfParallelism { get; init; } = 1;
}
