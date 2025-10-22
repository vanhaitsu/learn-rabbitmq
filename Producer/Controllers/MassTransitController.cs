using Microsoft.AspNetCore.Mvc;

namespace Producer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MassTransitController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string message)
    {
        return Ok();
    }
}