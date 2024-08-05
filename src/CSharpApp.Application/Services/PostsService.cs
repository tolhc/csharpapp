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
    
    public async Task<PostRecord?> GetPostById(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<PostRecord>($"posts/{id}", cancellationToken);

        return response;
    }

    public async Task<ReadOnlyCollection<PostRecord>> GetAllPosts(CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<List<PostRecord>>("posts", cancellationToken);

        return response?.AsReadOnly() ?? ReadOnlyCollection<PostRecord>.Empty;
    }

    public async Task<PostRecord?> CreatePost(PostRecord postRecord, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.PostAsync("posts", postRecord, cancellationToken);
        return response;
    }
    
    public async Task DeletePostById(int id, CancellationToken cancellationToken) => await _httpClientWrapper.DeleteAsync<PostRecord?>($"posts/{id}", cancellationToken);
    
}