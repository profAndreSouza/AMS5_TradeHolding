using SimpleBroker.Broker;

namespace SimpleBroker.Demo;

public static class DemoSetup
{
    public static void Run(Broker.SimpleBroker broker, CancellationTokenSource cts)
    {
        // --------- Fila (work queue) ---------
        var queue = "emails";
        broker.DeclareQueue(queue);

        Console.WriteLine("\nModo FILA (Work Queue):");
        Console.WriteLine("Producer → Queue (emails) → Worker1 / Worker2 (somente um processa cada mensagem)\n");

        broker.SubscribeQueue(queue, async msg =>
        {
            await Task.Delay(100);
            Console.WriteLine($"   [Worker1] recebeu: {msg.Body}");
            return true;
        }, new ConsumerOptions { DegreeOfParallelism = 1 }, cts.Token);

        broker.SubscribeQueue(queue, async msg =>
        {
            await Task.Delay(50);
            Console.WriteLine($"   [Worker2] recebeu: {msg.Body}");
            return true;
        }, new ConsumerOptions { DegreeOfParallelism = 1 }, cts.Token);

        _ = Task.Run(async () =>
        {
            for (int i = 1; i <= 6; i++)
            {
                Console.WriteLine($"→ Producer publicou: Email {i}");
                await broker.PublishToQueueAsync(queue, $"Email {i}");
                await Task.Delay(80);
            }
        });

        // --------- Pub/Sub ---------
        var topic = "noticias";
        broker.DeclareTopic(topic);

        Console.WriteLine("\nModo PUB/SUB (Broadcast):");
        Console.WriteLine("Publisher → Topic (noticias) → NewsService + AnalyticsService (todos recebem a mesma mensagem)\n");

        broker.SubscribeTopic(topic, async msg =>
        {
            Console.WriteLine($"   [NewsService] recebeu: {msg.Body}");
            await Task.CompletedTask;
        }, cts.Token);

        broker.SubscribeTopic(topic, async msg =>
        {
            Console.WriteLine($"   [AnalyticsService] logou: {msg.Body}");
            await Task.CompletedTask;
        }, cts.Token);

        _ = Task.Run(async () =>
        {
            await Task.Delay(1000);
            broker.PublishToTopic(topic, "Artigo novo publicado!");
            broker.PublishToTopic(topic, "Atualização de sistema disponível.");
        });
    }
}
