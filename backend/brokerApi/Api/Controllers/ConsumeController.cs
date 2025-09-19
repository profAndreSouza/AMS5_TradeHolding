using Microsoft.AspNetCore.Mvc;
using System.Text;
using Services;

namespace Api.Controllers
{


    [ApiController]
    [Route("consume")]
    public class ConsumeController : ControllerBase
    {
        private readonly Broker _broker;

        public ConsumeController(Broker broker)
        {
            _broker = broker;

            // garante uma fila default ligada à exchange
            _broker.DeclareQueue("task_queue");
            _broker.Bind("amq.direct", "task", "task_queue");
        }

        [HttpGet("{queueName}")]
        public async Task<IActionResult> Consume(string queueName)
        {
            var queue = _broker.GetQueue(queueName);
            if (queue == null) return NotFound("Queue not found");

            using var cts = new CancellationTokenSource(1000); // timeout 1s
            var msg = await queue.DequeueAsync(cts.Token);
            if (msg == null) return NoContent();

            return Ok(new
            {
                msg.Id,
                msg.RoutingKey,
                Body = Encoding.UTF8.GetString(msg.Body),
                msg.DeliveryCount
            });
        }
    }
}