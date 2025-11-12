using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqPlay.PublisherApi.Controllers
{
    [ApiController]
    public class PublisherController
    {
        public record PublishRequest
        {
            public required string Message { get; init; }
        }

        [HttpPost("/publish")]
        public async Task<IActionResult> PublishMessage(
            [FromBody] PublishRequest request,
            IConnection connection)
        {
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "hello",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            var body = Encoding.UTF8.GetBytes(request.Message);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "hello",
                body: body);

            return new OkObjectResult("Message published");
        }
    }
}
