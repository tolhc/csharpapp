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
        
        if (response.IsFailure)
        {
            _logger.LogError("Failure in {service} when trying to {method} for post id {id} with error {applicationError} and status code {statusCode}",
                nameof(PostsService), nameof(GetPostById), id, response.Error!.Description, response.Error.StatusCode);
        }
        
        return response;
    }

    public async Task<Result<ReadOnlyCollection<PostRecord>, ApplicationError>> GetAllPosts(CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<List<PostRecord>>("posts", cancellationToken);

        // ReSharper disable once InvertIf (justification: more consistent this way)
        if (response.IsFailure)
        {
            _logger.LogError("Failure in {service} when trying to {method} with error {applicationError} and status code {statusCode}",
                nameof(PostsService), nameof(GetAllPosts), response.Error!.Description, response.Error.StatusCode);
            
            return response.Error;
        }
        
        return response.Value?.AsReadOnly() ?? ReadOnlyCollection<PostRecord>.Empty;
    }

    public async Task<Result<PostRecord?, ApplicationError>> CreatePost(PostRecord postRecord, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.PostAsync("posts", postRecord, cancellationToken);
        
        if (response.IsFailure)
        {
            _logger.LogError("Failure in {service} when trying to {method} for post with title {postTitle} with error {applicationError} and status code {statusCode}",
                nameof(PostsService), nameof(CreatePost), postRecord.Title, response.Error!.Description, response.Error.StatusCode);
        }
        
        return response;
    }

    public async Task<UnitResult<ApplicationError>> DeletePostById(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.DeleteAsync<PostRecord?>($"posts/{id}", cancellationToken);
        
        // ReSharper disable once InvertIf (justification: more consistent this way)
        if (response.IsFailure)
        {
            _logger.LogError("Failure in {service} when trying to {method} for post id {id} with error {applicationError} and status code {statusCode}",
                nameof(PostsService), nameof(DeletePostById), id, response.Error!.Description, response.Error.StatusCode);
            return response.Error;
        }
        
        return UnitResult<ApplicationError>.Success();
    }

}