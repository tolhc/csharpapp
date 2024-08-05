using CSharpApp.Application;
using CSharpApp.Core.Dtos;
using CSharpApp.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

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
    .WithOpenApi()
    .WithTags("Todos");

app.MapGet("/todos/{id}", async ([FromRoute] int id, ITodoService todoService, CancellationToken cancellationToken) =>
    {
        var todos = await todoService.GetTodoById(id ,cancellationToken);
        return todos;
    })
    .WithName("GetTodosById")
    .WithOpenApi()
    .WithTags("Todos");


app.MapGet("/posts", async (IPostsService postsService, CancellationToken cancellationToken) =>
    {
        var todos = await postsService.GetAllPosts(cancellationToken);
        return todos;
    })
    .WithName("GetPosts")
    .WithOpenApi()
    .WithTags("Posts");

app.MapGet("/posts/{id}", async ([FromRoute] int id, IPostsService postsService, CancellationToken cancellationToken) =>
    {
        var todos = await postsService.GetPostById(id, cancellationToken);
        return todos;
    })
    .WithName("GetPostsById")
    .WithOpenApi()
    .WithTags("Posts");


app.MapPost("/posts", async ([FromBody] PostRecord post, IPostsService postsService, CancellationToken cancellationToken) =>
    {
        var postResult = await postsService.CreatePost(post, cancellationToken);
        return Results.Created($"posts/{postResult!.Id}", postResult);
    })
    .WithName("CreatePosts")
    .WithOpenApi()
    .WithTags("Posts");

app.MapDelete("/posts/{id}", async ([FromRoute] int id, IPostsService postsService, CancellationToken cancellationToken) =>
    {
        await postsService.DeletePostById(id, cancellationToken);
        return Results.Ok();
    })
    .WithName("DeletePostsById")
    .WithOpenApi()
    .WithTags("Posts");

app.Run();