using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class WalletMessagePublisher
{
    private readonly IConnection _connection;

    public WalletMessagePublisher(IConnection connection)
    {
        _connection = connection;
    }

    public Task PublishAsync(string queueName, object message)
    {
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

        Console.WriteLine($"[x] Published to {queueName}: {json}");
        return Task.CompletedTask;
    }
}
