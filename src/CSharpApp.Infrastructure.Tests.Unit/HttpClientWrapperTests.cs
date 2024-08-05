using System.Net;
using System.Text.Json;
using CSharpApp.Core.Dtos;
using CSharpApp.Infrastructure.Clients;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace CSharpApp.Infrastructure.Tests.Unit;

public class HttpClientWrapperTests
{
    private readonly Mock<HttpMessageHandler> _httpHandlerMock = new(MockBehavior.Strict);
    private readonly Mock<ILogger<HttpClientWrapper>> _loggerMock = new();
    private readonly HttpClient _clientMock;
    
    private const string _mockedResponse = """
                                           {
                                               "userId": 1,
                                               "id": 1,
                                               "title": "delectus aut autem",
                                               "completed": false
                                           }
                                           """;

    public HttpClientWrapperTests()
    {
        _clientMock = new HttpClient(_httpHandlerMock.Object)
        {
            BaseAddress = new Uri("http://fakeaddress.com")
        };
    }

    [Fact]
    public async Task GetAsync_WhenSuccessResponse_ShouldReturnSuccessfulResult()
    {
        // Arrange
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_mockedResponse)
            })
            .Verifiable();

        var expectedResult = new TodoRecord(1, 1, "delectus aut autem", false);
        
        // Act

        var sut = new HttpClientWrapper(_clientMock, _loggerMock.Object);
        var result = await sut.GetAsync<TodoRecord>(It.IsAny<string>(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.Be(expectedResult);

        VerifyHttpHandlerMock(_httpHandlerMock, HttpMethod.Get, times: 1);
        
        _httpHandlerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAsync_WhenNoSuccessCode_ShouldReturnApplicationErrorResult()
    {
        // Arrange
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = null
            })
            .Verifiable();

        // Act

        var sut = new HttpClientWrapper(_clientMock, _loggerMock.Object);
        var result = await sut.GetAsync<TodoRecord>("myendpoint", CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Error.Description.Should().Be("Unsuccessful status code when trying to GET for endpoint myendpoint");
        
        VerifyHttpHandlerMock(_httpHandlerMock, HttpMethod.Get, times: 1);
        
        _httpHandlerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAsync_WhenClientThrows_ShouldReturnApplicationErrorResult()
    {
        // Arrange
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TimeoutException("random timeout"))
            .Verifiable();

        // Act

        var sut = new HttpClientWrapper(_clientMock, _loggerMock.Object);
        var result = await sut.GetAsync<TodoRecord>("myendpoint", CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Error.Description.Should().Be("Exception when trying to GET for endpoint myendpoint");
        
        VerifyHttpHandlerMock(_httpHandlerMock, HttpMethod.Get, times: 1);
        
        _httpHandlerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task PostAsync_WhenSuccessResponse_ShouldReturnSuccessfulResult()
    {
        // Arrange
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_mockedResponse)
            })
            .Verifiable();

        var expectedResult = new TodoRecord(1, 1, "delectus aut autem", false);
        var dataToPost = new TodoRecord(10, 10, "test test", true);
        
        // Act

        var sut = new HttpClientWrapper(_clientMock, _loggerMock.Object);
        var result = await sut.PostAsync(It.IsAny<string>(), dataToPost, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.Be(expectedResult);

        VerifyHttpHandlerMock(_httpHandlerMock, HttpMethod.Post, times: 1, payload: dataToPost);
        
        _httpHandlerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task PutAsync_WhenSuccessResponse_ShouldReturnSuccessfulResult()
    {
        // Arrange
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_mockedResponse)
            })
            .Verifiable();

        var expectedResult = new TodoRecord(1, 1, "delectus aut autem", false);
        var dataToPost = new TodoRecord(10, 10, "test test", true);
        
        // Act

        var sut = new HttpClientWrapper(_clientMock, _loggerMock.Object);
        var result = await sut.PutAsync(It.IsAny<string>(), dataToPost, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.Be(expectedResult);

        VerifyHttpHandlerMock(_httpHandlerMock, HttpMethod.Post, times: 1, payload: dataToPost);
        
        _httpHandlerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task DeleteAsync_WhenSuccessResponse_ShouldReturnSuccessfulResult()
    {
        // Arrange
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_mockedResponse)
            })
            .Verifiable();

        var expectedResult = new TodoRecord(1, 1, "delectus aut autem", false);
        
        // Act

        var sut = new HttpClientWrapper(_clientMock, _loggerMock.Object);
        var result = await sut.DeleteAsync<TodoRecord>(It.IsAny<string>(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull().And.Be(expectedResult);

        VerifyHttpHandlerMock(_httpHandlerMock, HttpMethod.Post, times: 1);
        
        _httpHandlerMock.VerifyNoOtherCalls();
    }

    private static void VerifyHttpHandlerMock(Mock<HttpMessageHandler> httpHandlerMock, HttpMethod expectedMethod, int times, TodoRecord? payload = null)
    {
        httpHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(times),
            ItExpr.Is<HttpRequestMessage>(req =>
                // ReSharper disable once SimplifyConditionalTernaryExpression (justification: it makes it weirder tbh)
                req.Method == expectedMethod
                && req.RequestUri!.ToString().Equals("http://fakeaddress.com/")
                && req.Headers.Count() == 1 
                && req.Headers.Accept.First().MediaType == "application/json"
                && payload != null ? VerifyPayload(req.Content!, payload) : true
                ),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    private static bool VerifyPayload(HttpContent content, TodoRecord payload)
    {
        var stringContent = content.ReadAsStringAsync().GetAwaiter().GetResult(); // not ideal but doesn't matter for the assertion here really
        var result = JsonSerializer.Deserialize<TodoRecord>(stringContent);
        return result == payload;
    }
}