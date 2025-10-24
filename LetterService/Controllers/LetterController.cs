using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace LetterService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LetterController(IPublishEndpoint publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string message)
        {
            await publishEndpoint.Publish(new Letter { Body = message });

            return Ok();
        }
    }
}