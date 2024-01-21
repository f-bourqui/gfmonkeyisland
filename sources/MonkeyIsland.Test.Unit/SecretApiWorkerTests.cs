using Microsoft.Extensions.Options;
using MonkeyIsland.Core;
using MonkeyIsland.Core.Models;
using MonkeyIsland.Core.Settings;
using Moq;
using Moq.Protected;
using System.Net.Http.Json;

namespace MonkeyIsland.Test.Unit;

[TestClass]
public class SecretApiWorkerTests
{
    private MagicNumberHandler? _handler;
    private SecretApiHttpClient? _secretApiHttpClient;
    private Mock<HttpMessageHandler>? _messageHandlerMock;

    [TestInitialize]
    public void TestInitialize()
    {
        _messageHandlerMock = new Mock<HttpMessageHandler>();
        _secretApiHttpClient = new SecretApiHttpClient(null, Options.Create(new ApiSettings { Url = "https://test.url", Key = "testKey", CallbackUrl = "callbackUrl" }), new HttpClient(_messageHandlerMock.Object));
        _handler = new MagicNumberHandler(null, _secretApiHttpClient);
    }

    [TestMethod]
    public async Task TestMagicNumberHandler_success()
    {
        MagicNumberCallback? callback = null;

        _messageHandlerMock!.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage
            {
                Content = JsonContent.Create(new MagicNumbersResponse(new[] { 1, 2, 3, 4, 5 }))
            })
            .Verifiable();
        _messageHandlerMock!.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>()).
            Callback(async (HttpRequestMessage request, CancellationToken token) =>
            {
                if (request.Content is { })
                {
                    callback = await request.Content.ReadFromJsonAsync<MagicNumberCallback>();
                }
            }).ReturnsAsync((HttpRequestMessage request, CancellationToken token) => new HttpResponseMessage
            ());


        await _handler!.DoYourMagic();

        _messageHandlerMock!.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>());
        _messageHandlerMock!.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>());
        Assert.IsNotNull(callback);
        Assert.AreEqual(15, callback.sum);
        Assert.AreEqual("callbackUrl", callback.callBackUrl);

    }



}