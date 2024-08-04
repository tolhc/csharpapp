using CSharpApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpApp.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
    }
}