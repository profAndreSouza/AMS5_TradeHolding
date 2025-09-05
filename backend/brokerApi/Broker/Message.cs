namespace SimpleBroker.Broker;

public record Message(
    string Body,
    string Id,
    int DeliveryCount,
    DateTime EnqueuedAtUtc
)
{
    public static Message Create(string body) => new(
        Body: body,
        Id: Guid.NewGuid().ToString("N"),
        DeliveryCount: 0,
        EnqueuedAtUtc: DateTime.UtcNow
    );

    public Message WithIncrementedDelivery() => this with { DeliveryCount = DeliveryCount + 1 };
}
