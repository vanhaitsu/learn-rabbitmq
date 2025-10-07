using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private const string QueueName = "queue_1";
        private const string RoutingKey = "routing_1";
        private const string ExchangeName = "exchange_1";
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string message)
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
            await channel.QueueDeclareAsync(QueueName, false, false, false, null);
            await channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey, null);
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(
                exchange: ExchangeName,
                routingKey: RoutingKey,
                body: body
            );
            Console.WriteLine($"Sent {message}");
            await channel.CloseAsync();
            
            return Ok();
        }
    }
}