using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace WordleGame.Test;

[TestFixture]
public class WebApiHandler_Test
{

    [Test]
    public async Task test()
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
        {
            Content = JsonContent.Create(new
            {
                Strings = new[] { "TEST1", "TEST2", "TEST3", "TEST4", "TEST5" }
            })
        };
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new System.Uri("https://api.frontendexpert.io/api/fe/wordle-words")
        };
        WebApiHandler webApiHandler = new WebApiHandler(httpClient);
        var result = await webApiHandler.GetWords();
        var resultList = result.Split(',');
        Assert.AreEqual("TEST1",resultList[0]);
    }
}