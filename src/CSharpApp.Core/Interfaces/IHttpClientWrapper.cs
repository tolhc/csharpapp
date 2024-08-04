namespace CSharpApp.Core.Interfaces;

public interface IHttpClientWrapper
{
    Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken);
    Task<T?> PostAsync<T>(string endpoint, T data, CancellationToken cancellationToken);
    Task<T?> PutAsync<T>(string endpoint, T data, CancellationToken cancellationToken);
    Task<T?> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken);
}