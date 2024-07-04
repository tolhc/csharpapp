namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly HttpClient _client;

    private readonly string? _baseUrl;

    public TodoService(ILogger<TodoService> logger, 
        IConfiguration configuration)
    {
        _logger = logger;
        _client = new HttpClient();
        _baseUrl = configuration["BaseUrl"];
    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        _client.BaseAddress = new Uri(_baseUrl!);
        var response = await _client.GetFromJsonAsync<TodoRecord>($"todos/{id}");

        return response;
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        _client.BaseAddress = new Uri(_baseUrl!);
        var response = await _client.GetFromJsonAsync<List<TodoRecord>>($"todos");

        return response!.AsReadOnly();
    }
}