namespace CSharpApp.Core.Interfaces;

public interface ITodoService
{
    Task<Result<TodoRecord?, ApplicationError>> GetTodoById(int id, CancellationToken cancellationToken);
    Task<Result<ReadOnlyCollection<TodoRecord>, ApplicationError>> GetAllTodos(CancellationToken cancellationToken);
}