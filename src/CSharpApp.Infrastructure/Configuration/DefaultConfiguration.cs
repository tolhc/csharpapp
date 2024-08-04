namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ITodoService, TodoService>(c => 
            c.BaseAddress = new Uri(configuration["BaseUrl"] 
                                    ?? throw new InvalidOperationException("BaseUrl is missing from Configuration")));
        
        return services;
    }
}