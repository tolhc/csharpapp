namespace CSharpApp.Core.Interfaces;

public interface ITodoService
{
    Task<TodoRecord?> GetTodoById(int id);
    Task<ReadOnlyCollection<TodoRecord>> GetAllTodos();
}