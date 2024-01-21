using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MonkeyIsland.Core.Models;
using MonkeyIsland.Core.Settings;
using System.Net.Http.Json;

namespace MonkeyIsland.Core;

public class SecretApiHttpClient
{
    private readonly ILogger<SecretApiHttpClient> _logger;
    private readonly ApiSettings _apiSettings;
    private readonly HttpClient _httpClient;

    public SecretApiHttpClient(ILogger<SecretApiHttpClient>? logger, IOptions<ApiSettings> apiSettingsOptions, HttpClient httpClient)
    {
        _logger = logger ?? NullLogger<SecretApiHttpClient>.Instance;
        _apiSettings = apiSettingsOptions.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_apiSettings.Url);
    }

    public async Task<MagicNumbersResponse?> GetMagicNumbersAsync()
    {
        return await _httpClient.GetFromJsonAsync<MagicNumbersResponse>(_apiSettings.Key);
    }

    public async Task SendResult(int result)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.Key, new MagicNumberCallback(result, _apiSettings.CallbackUrl));
        response.EnsureSuccessStatusCode();
    }
}