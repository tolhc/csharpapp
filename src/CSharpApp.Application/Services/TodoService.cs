namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly IHttpClientWrapper _httpClientWrapper;

    public TodoService(ILogger<TodoService> logger, 
        IHttpClientWrapper httpClient)
    {
        _logger = logger;
        _httpClientWrapper = httpClient;
    }

    public async Task<TodoRecord?> GetTodoById(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<TodoRecord>($"todos/{id}", cancellationToken);

        return response;
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos(CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<List<TodoRecord>>($"todos", cancellationToken);

        return response?.AsReadOnly() ?? ReadOnlyCollection<TodoRecord>.Empty;
    }
}