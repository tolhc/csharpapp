using CSharpApp.Application;
using CSharpApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddHttpClients(builder.Configuration);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/todos", async (ITodoService todoService, CancellationToken cancellationToken) =>
    {
        var todos = await todoService.GetAllTodos(cancellationToken);
        return todos;
    })
    .WithName("GetTodos")
    .WithOpenApi();

app.MapGet("/todos/{id}", async ([FromRoute] int id, ITodoService todoService, CancellationToken cancellationToken) =>
    {
        var todos = await todoService.GetTodoById(id ,cancellationToken);
        return todos;
    })
    .WithName("GetTodosById")
    .WithOpenApi();

app.Run();