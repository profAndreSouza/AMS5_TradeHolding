using Microsoft.AspNetCore.Mvc;
using System.Text;
using Core;
using Services;

namespace Api.Controllers
{

    [ApiController]
    [Route("publish")]
    public class PublishController : ControllerBase
    {
        private readonly Broker _broker;

        public PublishController(Broker broker)
        {
            _broker = broker;

            // garante uma exchange default
            _broker.DeclareExchange("amq.direct", ExchangeType.Direct);
        }

        [HttpPost]
        public async Task<IActionResult> Publish([FromBody] PublishRequest req)
        {
            var message = new Message(req.RoutingKey, Encoding.UTF8.GetBytes(req.Body));
            await _broker.PublishAsync(req.Exchange ?? "amq.direct", message);

            return Ok(new { Status = "Published", MessageId = message.Id });
        }
    }

    public record PublishRequest(string? Exchange, string RoutingKey, string Body);

}