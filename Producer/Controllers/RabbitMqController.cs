using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using Constants = Shared.Common.Constants;

namespace Producer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RabbitMqController(IConfiguration configuration, IRabbitMqService rabbitMqService)
    : ControllerBase
{
    private const string ExchangeName = "exchange_1";
    private const string RoutingKey = "routing_1";
    private const string QueueName = "queue_1";

    private readonly string _host = configuration[Constants.RabbitMqHostPath] ??
                                    throw new NullReferenceException(Constants.RabbitMqHostPath);

    private readonly string _password = configuration[Constants.RabbitMqPasswordPath] ??
                                        throw new NullReferenceException(Constants.RabbitMqPasswordPath);

    private readonly string _username = configuration[Constants.RabbitMqUsernamePath] ??
                                        throw new NullReferenceException(Constants.RabbitMqUsernamePath);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string message)
    {
        await rabbitMqService.InitializeAsync(_host, _username, _password);
        await rabbitMqService.PublishAsync(ExchangeName, QueueName, RoutingKey, message);

        return Ok();
    }
}