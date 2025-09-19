using Core;

namespace Services
{

    public class Consumer
    {
        private readonly IBroker _broker;
        private readonly IQueue _queue;
        private readonly Func<IMessage, Task<bool>> _processor; // returns true if ack, false if nack
        private readonly int _maxRetries;

        public Consumer(IBroker broker, string queueName, Func<IMessage, Task<bool>> processor, int maxRetries = 3)
        {
            _broker = broker;
            _queue = broker.GetQueue(queueName) ?? throw new ArgumentException("Queue not found");
            _processor = processor;
            _maxRetries = maxRetries;
        }

        public async Task StartAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var msg = await _queue.DequeueAsync(ct);
                if (msg == null) continue;

                try
                {
                    var success = await _processor(msg);
                    if (!success)
                    {
                        msg.DeliveryCount++;
                        if (msg.DeliveryCount > _maxRetries)
                        {
                            await _broker.MoveToDeadLetterAsync(msg);
                            Console.WriteLine($"Moved to DLQ: {msg.Id}");
                        }
                        else
                        {
                            // re-enqueue with backoff (simple)
                            await Task.Delay(500);
                            await _queue.EnqueueAsync(msg);
                            Console.WriteLine($"Requeued message {msg.Id}, deliveryCount={msg.DeliveryCount}");
                        }
                    }
                    else
                    {
                        // ack - nothing to do in our in-memory model
                        Console.WriteLine($"Processed and acked {msg.Id}");
                    }
                }
                catch (Exception ex)
                {
                    // treat as nack
                    msg.DeliveryCount++;
                    Console.WriteLine($"Processor threw: {ex.Message}. deliveryCount={msg.DeliveryCount}");
                    if (msg.DeliveryCount > _maxRetries)
                        await _broker.MoveToDeadLetterAsync(msg);
                    else
                        await _queue.EnqueueAsync(msg);
                }
            }
        }
    }
}