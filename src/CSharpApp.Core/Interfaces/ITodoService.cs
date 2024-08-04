namespace CSharpApp.Core.Interfaces;

public interface ITodoService
{
    Task<TodoRecord?> GetTodoById(int id, CancellationToken cancellationToken);
    Task<ReadOnlyCollection<TodoRecord>> GetAllTodos(CancellationToken cancellationToken);
}