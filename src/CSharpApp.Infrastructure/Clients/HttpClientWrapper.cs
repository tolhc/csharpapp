namespace CSharpApp.Infrastructure.Clients;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpClientWrapper> _logger;
    
    public HttpClientWrapper(HttpClient httpClient, ILogger<HttpClientWrapper> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<Result<T?, ApplicationError>> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        return await SendAsync<T?>(HttpMethod.Get, endpoint, cancellationToken: cancellationToken, httpCompletionOption: HttpCompletionOption.ResponseHeadersRead);
    }

    public async Task<Result<T?, ApplicationError>> PostAsync<T>(string endpoint, T data, CancellationToken cancellationToken)
    {
        return await SendAsync(HttpMethod.Post, endpoint, data, cancellationToken);
    }

    public async Task<Result<T?, ApplicationError>> PutAsync<T>(string endpoint, T data, CancellationToken cancellationToken)
    {
        return await SendAsync(HttpMethod.Put, endpoint, data, cancellationToken);
    }

    public async Task<Result<T?, ApplicationError>> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        return await SendAsync<T?>(HttpMethod.Delete, endpoint, cancellationToken: cancellationToken);
    }

    private async Task<Result<T?, ApplicationError>> SendAsync<T>(HttpMethod method, string endpoint, T? body = default, 
        CancellationToken cancellationToken = default,
        HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
    {
        try
        {
            var httpRequestMessage = GetRequestMessage(method, endpoint, body);
            using var response = await _httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return new ApplicationError($"Unsuccessful status code when trying to {method} for endpoint {endpoint}", response.StatusCode);
            }
            
            var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<T?>(responseStream, cancellationToken: cancellationToken) ?? default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception when trying to {method} for endpoint {endpoint}", method, endpoint);
            return new ApplicationError($"Exception when trying to {method} for endpoint {endpoint}", HttpStatusCode.InternalServerError);
        }
        
    }

    private static HttpRequestMessage GetRequestMessage<T>(HttpMethod method, string endpoint, T? body)
    {
        return new HttpRequestMessage(method, endpoint)
        {
            Content = body is null
                ? default
                : new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"),
            Headers =
            {
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
    }
}