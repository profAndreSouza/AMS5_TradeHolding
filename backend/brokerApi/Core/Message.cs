namespace Core
{

    public class Message : IMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string RoutingKey { get; }
        public byte[] Body { get; }
        public int DeliveryCount { get; set; } = 0;
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        public Message(string routingKey, byte[] body)
        {
            RoutingKey = routingKey;
            Body = body;
        }

        public override string ToString() => $"Message(Id={Id}, RoutingKey={RoutingKey}, DeliveryCount={DeliveryCount})";
    }
}