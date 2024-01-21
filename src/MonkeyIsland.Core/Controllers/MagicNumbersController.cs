using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MonkeyIsland.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class MagicNumbersController(ILogger<MagicNumbersController>? logger) : ControllerBase
{
    private readonly ILogger<MagicNumbersController> _logger = logger ?? NullLogger<MagicNumbersController>.Instance;

    [HttpGet()]
    public async Task<IActionResult> Ping()
    {
        return Ok("Pong");
    }

    [HttpPost()]
    public void MagicNumbersCallback(string key)
    {
        _logger.LogInformation("Received key is : {key} ", key);
    }

    [HttpPost("callback")]
    public async Task<IActionResult> HandleUknownDataCallback()
    {
        try
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string rawContent = await reader.ReadToEndAsync();
                _logger.LogInformation($"Raw Callback Data: {rawContent}");
            }

            return Ok("Callback handled successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling callback");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
