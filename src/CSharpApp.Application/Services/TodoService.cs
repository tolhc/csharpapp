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
        
        if (response.IsFailure)
        {
            _logger.LogError("Failure in {service} when trying to {method} for todo id {id} with error {applicationError} and status code {statusCode}",
                nameof(TodoService), nameof(GetTodoById), id, response.Error!.Description, response.Error.StatusCode);
        }
        
        return response;
    }

    public async Task<Result<ReadOnlyCollection<TodoRecord>, ApplicationError>> GetAllTodos(CancellationToken cancellationToken)
    {
        var response = await _httpClientWrapper.GetAsync<List<TodoRecord>>($"todos", cancellationToken);

        // ReSharper disable once InvertIf (justification: more consistent this way)
        if (response.IsFailure)
        {
            _logger.LogError("Failure in {service} when trying to {method} with error {applicationError} and status code {statusCode}",
                nameof(TodoService), nameof(GetAllTodos), response.Error!.Description, response.Error.StatusCode);
            
            return response.Error;
        }
        
        return response.Value?.AsReadOnly() ?? ReadOnlyCollection<TodoRecord>.Empty;
    }
}