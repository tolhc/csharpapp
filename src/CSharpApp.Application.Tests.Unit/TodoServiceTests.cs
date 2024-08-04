using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CSharpApp.Application.Tests.Unit;

public class TodoServiceTests
{
    private readonly Mock<ILogger<TodoService>> _loggerMock = new();
    private readonly Mock<IHttpClientWrapper> _clientMock = new();
    
    [Fact]
    public async Task GetTodoById_WhenClientReturns_ShouldReturnSuccessfully()
    {
        // Arrange
        var expected = new TodoRecord(1, 2, "title", true);
        _clientMock.Setup(c => c.GetAsync<TodoRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var sut = new TodoService(_loggerMock.Object, _clientMock.Object);
        var result = await sut.GetTodoById(2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.Be(expected);
        
        _clientMock.Verify(c => c.GetAsync<TodoRecord>("todos/2", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetTodoById_WhenClientThrows_ShouldThrow() //TODO: fix when introducing Result pattern
    {
        // Arrange
        var expected = new TodoRecord(1, 2, "title", true);
        _clientMock.Setup(c => c.GetAsync<TodoRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        // Act
        var sut = new TodoService(_loggerMock.Object, _clientMock.Object);
        var resultFunc = async () => await sut.GetTodoById(2, CancellationToken.None);

        // Assert
        await resultFunc.Should().ThrowAsync<HttpRequestException>();
        
        _clientMock.Verify(c => c.GetAsync<TodoRecord>("todos/2", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAllTodos_WhenClientReturns_ShouldReturnSuccessfully()
    {
        // Arrange
        var expected = new List<TodoRecord>()
        {
            new(1, 2, "title", true), 
            new(1, 5, "another title", false)
        };
        _clientMock.Setup(c => c.GetAsync<List<TodoRecord>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var sut = new TodoService(_loggerMock.Object, _clientMock.Object);
        var result = await sut.GetAllTodos(CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.NotBeEmpty().And.BeEquivalentTo(expected);
        
        _clientMock.Verify(c => c.GetAsync<List<TodoRecord>>("todos", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAllTodos_WhenClientReturnsNull_ShouldReturnEmptyCollection()
    {
        // Arrange
        _clientMock.Setup(c => c.GetAsync<List<TodoRecord>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<TodoRecord>)null!);

        // Act
        var sut = new TodoService(_loggerMock.Object, _clientMock.Object);
        var result = await sut.GetAllTodos(CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
        
        _clientMock.Verify(c => c.GetAsync<List<TodoRecord>>("todos", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAllTodos_WhenClientThrows_ShouldThrow() //TODO: fix when introducing Result pattern
    {
        // Arrange
        _clientMock.Setup(c => c.GetAsync<List<TodoRecord>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        // Act
        var sut = new TodoService(_loggerMock.Object, _clientMock.Object);
        var resultFunc = async () => await sut.GetAllTodos(CancellationToken.None);

        // Assert
        await resultFunc.Should().ThrowAsync<HttpRequestException>();
        
        _clientMock.Verify(c => c.GetAsync<List<TodoRecord>>("todos", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
}