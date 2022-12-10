using System.Net;
using Moq.Protected;
using Microsoft.Extensions.Configuration;

namespace GetBusy.ChuckNorrisApi.Tests;

public class RandomChuckNorrisJokeTest
{

    [Fact]
    public async void Get_CallsSendAsync_WhenCalled()
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                Content = new StringContent("{\"id\":\"1\", \"value\":\"Chuck Norris can’t test for equality because he has no equal.\"}"),
                StatusCode = HttpStatusCode.OK
            });
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["url"]).Returns("https://api.chucknorris.io/jokes/random");
        IDataProvider<JokeResponse> dataProvider = new RandomChuckNorrisJoke(httpClientFactory.Object, configuration.Object);

        JokeResponse? response = await dataProvider.Get();
        Assert.Equal("Chuck Norris can’t test for equality because he has no equal.", response?.Value);
        mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async void Get_ThrowsChuckNorrisException_WhenHttpResponseIsError()
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["url"]).Returns("https://api.chucknorris.io/jokes/random");
        IDataProvider<JokeResponse> dataProvider = new RandomChuckNorrisJoke(httpClientFactory.Object, configuration.Object);

        await Assert.ThrowsAsync<ChuckNorrisException>(() => dataProvider.Get());
    }
}
