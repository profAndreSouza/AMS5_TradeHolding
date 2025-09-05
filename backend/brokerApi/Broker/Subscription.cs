namespace SimpleBroker.Broker;

internal sealed class Subscription : IDisposable
{
    private readonly CancellationTokenSource _cts;
    private readonly List<Task> _workers;

    public Subscription(CancellationTokenSource cts, List<Task> workers)
    {
        _cts = cts; _workers = workers;
    }

    public void Dispose()
    {
        _cts.Cancel();
        try { Task.WaitAll(_workers.ToArray(), TimeSpan.FromSeconds(2)); } catch { }
        _cts.Dispose();
    }
}
