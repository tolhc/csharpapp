namespace CSharpApp.Infrastructure.Clients;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;
    
    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        return await SendAsync<T?>(HttpMethod.Get, endpoint, cancellationToken: cancellationToken, httpCompletionOption: HttpCompletionOption.ResponseHeadersRead);
    }

    public async Task<T?> PostAsync<T>(string endpoint, T data, CancellationToken cancellationToken)
    {
        return await SendAsync(HttpMethod.Post, endpoint, data, cancellationToken);
    }

    public async Task<T?> PutAsync<T>(string endpoint, T data, CancellationToken cancellationToken)
    {
        return await SendAsync(HttpMethod.Put, endpoint, data, cancellationToken);
    }

    public async Task<T?> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        return await SendAsync<T?>(HttpMethod.Delete, endpoint, cancellationToken: cancellationToken);
    }

    private async Task<T?> SendAsync<T>(HttpMethod method, string endpoint, T? body = default, 
        CancellationToken cancellationToken = default,
        HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
    {
        var httpRequestMessage = GetRequestMessage(method, endpoint, body);
        using var response = await _httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: cancellationToken) ?? default;
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