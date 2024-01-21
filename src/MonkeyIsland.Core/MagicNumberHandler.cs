using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MonkeyIsland.Core.Models;

namespace MonkeyIsland.Core
{
    public class MagicNumberHandler(ILogger<MagicNumberHandler>? logger, SecretApiHttpClient apiHttpClient)
    {

        private readonly ILogger<MagicNumberHandler> _logger = logger ?? NullLogger<MagicNumberHandler>.Instance;
        private readonly SecretApiHttpClient _apiHttpClient = apiHttpClient;

        public async Task DoYourMagic()
        {
            var magicNumbers = await _apiHttpClient.GetMagicNumbersAsync();
            if (magicNumbers is { })
            {
                var sum = magicNumbers.magicNumbers.Sum();
                _logger.LogInformation("The magic numbers are {magic_numbers} and the sum is {magic_sum}", magicNumbers.magicNumbers.Any() ? magicNumbers.magicNumbers.Select(x => $"{x}").Aggregate((x, y) => $"{x},{y}") : "Empty", sum);
                await _apiHttpClient.SendResult(sum);
            }
        }
    }
}
