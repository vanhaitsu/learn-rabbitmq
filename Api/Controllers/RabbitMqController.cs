using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private const string QueueName = "queue_1";
        private const string RoutingKey = "routing_1";
        private const string ExchangeName = "exchange_1";
        
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            // Declare connection and channel
            var factory = new ConnectionFactory
            {
                HostName = Shared.Constants.RabbitMqHost,
                UserName = Shared.Constants.RabbitMqUserName,
                Password = Shared.Constants.RabbitMqPassword
            };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            
            // Declare exchange, queue
            await channel.ExchangeDeclareAsync(ExchangeName,  ExchangeType.Direct);
            
            
            
            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            const string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(
                exchange: ExchangeName,
                routingKey: RoutingKey,
                body: body
            );
            Console.WriteLine($"Sent {message}");
            
            return Ok();
        }
    }
}