using CSharpApp.Core;

namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public TodoService(ILogger<TodoService> logger, 
        IHttpClientWrapper httpClientWrapper)
    {
        _logger = logger;
        _httpClientWrapper = httpClientWrapper;
    }

    public async Task<Result<TodoRecord?, ApplicationError>> GetTodoById(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<TodoRecord?>($"todos/{id}", cancellationToken);
        return response;
    }

    public async Task<Result<ReadOnlyCollection<TodoRecord>, ApplicationError>> GetAllTodos(CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<List<TodoRecord>>($"todos", cancellationToken);

        if (response.IsFailure)
        {
            return response.Error!;
        }
        
        return response.Value?.AsReadOnly() ?? ReadOnlyCollection<TodoRecord>.Empty;
    }
}