using Microsoft.Extensions.Logging.Abstractions;
using MonkeyIsland.Core;

namespace MonkeyIsland.HostedServices;

public class SecretApiWorker(ILogger<SecretApiWorker>? logger, MagicNumberHandler magicNumberHandler) : BackgroundService
{
    private readonly ILogger<SecretApiWorker> _logger = logger ?? NullLogger<SecretApiWorker>.Instance;
    private readonly MagicNumberHandler _magicNumberHandler = magicNumberHandler;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _magicNumberHandler.DoYourMagic();

        while (!stoppingToken.IsCancellationRequested)
        {
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}