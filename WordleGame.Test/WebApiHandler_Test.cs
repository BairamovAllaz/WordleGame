using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace WordleGame.Test;

[TestFixture]
public class WebApiHandler_Test
{
    [Test]
    public async Task GetWords_SendFakeRequest_ReturnsString()
    {
        var httpClient = CreateFakeHttpRequest(HttpStatusCode.Accepted);
        WebApiHandler webApiHandler = new WebApiHandler(httpClient);
        var result = await webApiHandler.GetWords();
        var convertedResult = WebApiHandler.ConvertToList(result);
        Assert.AreEqual("TEST1", convertedResult[0]);
    }
    [Test]
    public async Task GetWords_NotGetRequest_ReturnsEmptyString()
    {
        var httpClient = CreateFakeHttpRequest((HttpStatusCode.Unauthorized));
        WebApiHandler webApiHandler = new WebApiHandler(httpClient);
        var expected = string.Empty;
        var actual = await webApiHandler.GetWords();
        Assert.AreEqual(expected,actual);
    }
    
    [Test]
    public void ConvertToList_TakesJsonArray_ReturnsListOfString()
    {
        var fakeList = new[] { "TEST1", "TEST2", "TEST3", "TEST4", "TEST5" };
        var seriazlizedFakeList = JsonConvert.SerializeObject(fakeList); 
        var actuall = WebApiHandler.ConvertToList(seriazlizedFakeList);
        var expected = new List<string>()
        {
            "TEST1",
            "TEST2",
            "TEST3",
            "TEST4",
            "TEST5",
        };
        Assert.AreEqual(expected,actuall);
    }
    
    [Test]
    public void ConvertToList_TakesNull_ThrowsArgumentNullExpection()
    {
        Assert.Catch<ArgumentNullException>(() => WebApiHandler.ConvertToList(null));
    }
    
    private HttpClient CreateFakeHttpRequest(HttpStatusCode statusCode)
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
        {
            Content = JsonContent.Create(
                new[] {"TEST1","TEST2","TEST3","TEST5"}),
            StatusCode = statusCode
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
        return httpClient;
    }
}