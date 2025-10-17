using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuração do RabbitMQ
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
{
    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
    UserName = "admin",
    Password = "admin"
});

builder.Services.AddSingleton<IConnection>(sp => sp.GetRequiredService<IConnectionFactory>().CreateConnection());
builder.Services.AddSingleton<WalletMessagePublisher>();
builder.Services.AddHostedService<WalletMessageConsumer>();

var app = builder.Build();

app.MapGet("/", () => Results.Json(new { message = "Hello from WalletAPI" }));

app.MapPost("/publish", async (WalletMessagePublisher publisher) =>
{
    var msg = new { Id = Guid.NewGuid(), Value = Random.Shared.Next(1, 1000), Date = DateTime.UtcNow };
    await publisher.PublishAsync("wallet.queue", msg);
    return Results.Json(new { status = "Message published", msg });
});

app.Run();
