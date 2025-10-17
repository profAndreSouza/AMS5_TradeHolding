using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class WalletMessageConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private IModel _channel;

    public WalletMessageConsumer(IConnection connection)
    {
        _connection = connection;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "wallet.queue", durable: false, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var obj = JsonSerializer.Deserialize<JsonElement>(message);
                Console.WriteLine($"[✓] Received from wallet.queue: {obj}");
            }
            catch
            {
                Console.WriteLine($"[x] Invalid message: {message}");
            }

            await Task.Yield();
        };

        _channel.BasicConsume(queue: "wallet.queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
