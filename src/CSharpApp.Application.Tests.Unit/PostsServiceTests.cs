using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CSharpApp.Application.Tests.Unit;

public class PostsServiceTests
{
    private readonly Mock<ILogger<PostsService>> _loggerMock = new();
    private readonly Mock<IHttpClientWrapper> _clientMock = new();
    
    [Fact]
    public async Task GetPostById_WhenClientReturns_ShouldReturnSuccessfully()
    {
        // Arrange
        var expected = new PostRecord(1, 2, "post title", "post body");
        _clientMock.Setup(c => c.GetAsync<PostRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var result = await sut.GetPostById(2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.Be(expected);
        
        _clientMock.Verify(c => c.GetAsync<PostRecord>("posts/2", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetPostById_WhenClientThrows_ShouldThrow() //Posts: fix when introducing Result pattern
    {
        // Arrange
        var expected = new PostRecord(1, 2, "post title", "post body");
        _clientMock.Setup(c => c.GetAsync<PostRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var resultFunc = async () => await sut.GetPostById(2, CancellationToken.None);

        // Assert
        await resultFunc.Should().ThrowAsync<HttpRequestException>();
        
        _clientMock.Verify(c => c.GetAsync<PostRecord>("posts/2", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAllPosts_WhenClientReturns_ShouldReturnSuccessfully()
    {
        // Arrange
        var expected = new List<PostRecord>()
        {
            new(1, 2, "post title", "post body"),
            new(1, 5, "another post title", "another post body")
        };
        _clientMock.Setup(c => c.GetAsync<List<PostRecord>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var result = await sut.GetAllPosts(CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.NotBeEmpty().And.BeEquivalentTo(expected);
        
        _clientMock.Verify(c => c.GetAsync<List<PostRecord>>("posts", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAllPosts_WhenClientReturnsNull_ShouldReturnEmptyCollection()
    {
        // Arrange
        _clientMock.Setup(c => c.GetAsync<List<PostRecord>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<PostRecord>)null!);

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var result = await sut.GetAllPosts(CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
        
        _clientMock.Verify(c => c.GetAsync<List<PostRecord>>("posts", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task GetAllPosts_WhenClientThrows_ShouldThrow() //Posts: fix when introducing Result pattern
    {
        // Arrange
        _clientMock.Setup(c => c.GetAsync<List<PostRecord>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var resultFunc = async () => await sut.GetAllPosts(CancellationToken.None);

        // Assert
        await resultFunc.Should().ThrowAsync<HttpRequestException>();
        
        _clientMock.Verify(c => c.GetAsync<List<PostRecord>>("posts", It.IsAny<CancellationToken>()), Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task CreatePost_WhenClientReturns_ShouldReturnSuccessfully()
    {
        // Arrange
        var expected = new PostRecord(1, 2, "created post title", "created post body");
        _clientMock.Setup(c => c.PostAsync(It.IsAny<string>(), It.IsAny<PostRecord>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var posted = new PostRecord(1, 2, "post title to create", "post body to create");
        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var result = await sut.CreatePost(posted, CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.Be(expected);
        
        _clientMock.Verify(c => c.PostAsync("posts", It.Is<PostRecord>(r => r == posted), It.IsAny<CancellationToken>()),
            Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task CreatePost_WhenClientThrows_ShouldThrow() //Posts: fix when introducing Result pattern
    {
        // Arrange
        var expected = new PostRecord(1, 2, "post title", "post body");
        _clientMock.Setup(c => c.PostAsync(It.IsAny<string>(), It.IsAny<PostRecord>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        
        var posted = new PostRecord(1, 2, "post title to create", "post body to create");

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var resultFunc = async () => await sut.CreatePost(posted, CancellationToken.None);

        // Assert
        await resultFunc.Should().ThrowAsync<HttpRequestException>();
        
        _clientMock.Verify(c => c.PostAsync("posts", It.Is<PostRecord>(r => r == posted), It.IsAny<CancellationToken>()),
            Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task DeletePostById_WhenClientReturns_ShouldReturnSuccessfully()
    {
        // Arrange
        _clientMock.Setup(c => c.DeleteAsync<PostRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PostRecord(1, 2, "post title", "post body"));

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        await sut.DeletePostById(2, CancellationToken.None);

        // Assert
        _clientMock.Verify(c => c.DeleteAsync<PostRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task DeletePostById_WhenClientThrows_ShouldThrow() //Posts: fix when introducing Result pattern
    {
        // Arrange
        _clientMock.Setup(c => c.DeleteAsync<PostRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        // Act
        var sut = new PostsService(_loggerMock.Object, _clientMock.Object);
        var resultFunc = async () => await sut.DeletePostById(2, CancellationToken.None);

        // Assert
        await resultFunc.Should().ThrowAsync<HttpRequestException>();
        
        _clientMock.Verify(c => c.DeleteAsync<PostRecord>(It.IsAny<string>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        _clientMock.VerifyNoOtherCalls();
    }
}