using CSharpApp.Core;

namespace CSharpApp.Application.Services;

public class PostsService : IPostsService
{
    private readonly ILogger<PostsService> _logger;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public PostsService(ILogger<PostsService> logger, 
        IHttpClientWrapper httpClientWrapper)
    {
        _logger = logger;
        _httpClientWrapper = httpClientWrapper;
    }
    
    public async Task<Result<PostRecord?, ApplicationError>> GetPostById(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<PostRecord>($"posts/{id}", cancellationToken);

        return response;
    }

    public async Task<Result<ReadOnlyCollection<PostRecord>, ApplicationError>> GetAllPosts(CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<List<PostRecord>>("posts", cancellationToken);

        if (response.IsFailure)
        {
            return response.Error!;
        }
        
        return response.Value?.AsReadOnly() ?? ReadOnlyCollection<PostRecord>.Empty;
    }

    public async Task<Result<PostRecord?, ApplicationError>> CreatePost(PostRecord postRecord, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.PostAsync("posts", postRecord, cancellationToken);
        return response;
    }

    public async Task<UnitResult<ApplicationError>> DeletePostById(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.DeleteAsync<PostRecord?>($"posts/{id}", cancellationToken);
        return response.IsFailure ? response.Error! : UnitResult<ApplicationError>.Success();
    }

}