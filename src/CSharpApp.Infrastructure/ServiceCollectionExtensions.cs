namespace CSharpApp.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>(c => 
            c.BaseAddress = new Uri(configuration["BaseUrl"] 
                                    ?? throw new InvalidOperationException("BaseUrl is missing from Configuration")));
        
        return services;
    }
}