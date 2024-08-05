namespace CSharpApp.Core.Interfaces;

public interface IHttpClientWrapper
{
    Task<Result<T?, ApplicationError>> GetAsync<T>(string endpoint, CancellationToken cancellationToken);
    Task<Result<T?, ApplicationError>> PostAsync<T>(string endpoint, T data, CancellationToken cancellationToken);
    Task<Result<T?, ApplicationError>> PutAsync<T>(string endpoint, T data, CancellationToken cancellationToken);
    Task<Result<T?, ApplicationError>> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken);
}