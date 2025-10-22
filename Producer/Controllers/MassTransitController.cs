using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Producer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MassTransitController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string message)
    {
        await _publishEndpoint.Publish(new Message { Text = message });

        return Ok();
    }
}