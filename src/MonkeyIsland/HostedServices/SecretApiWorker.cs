using Microsoft.Extensions.Logging.Abstractions;
using MonkeyIsland.Core;

namespace MonkeyIsland.HostedServices;

/// <summary>
/// Hosted background worker to call do your magic every x min
/// </summary>
/// <param name="logger"></param>
/// <param name="magicNumberHandler"></param>
public class SecretApiWorker(ILogger<SecretApiWorker>? logger, MagicNumberHandler magicNumberHandler) : BackgroundService
{
    private readonly ILogger<SecretApiWorker> _logger = logger ?? NullLogger<SecretApiWorker>.Instance;
    private readonly MagicNumberHandler _magicNumberHandler = magicNumberHandler;

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting secret api routine");
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            await _magicNumberHandler.DoYourMagic();
        }
        _logger.LogInformation("End of secret api routine");
    }
}