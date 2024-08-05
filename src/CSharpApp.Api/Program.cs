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
        var todosResult = await todoService.GetAllTodos(cancellationToken);
        if (todosResult.IsFailure)
        {
            Results.StatusCode((int)todosResult.Error!.StatusCode);
        }
        return todosResult.Value;
    })
    .WithName("GetTodos")
    .WithOpenApi()
    .WithTags("Todos");

app.MapGet("/todos/{id}", async ([FromRoute] int id, ITodoService todoService, CancellationToken cancellationToken) =>
    {
        var todosResult = await todoService.GetTodoById(id ,cancellationToken);
        if (todosResult.IsFailure)
        {
            Results.StatusCode((int)todosResult.Error!.StatusCode);
        }
        return todosResult.Value;
    })
    .WithName("GetTodosById")
    .WithOpenApi()
    .WithTags("Todos");


app.MapGet("/posts", async (IPostsService postsService, CancellationToken cancellationToken) =>
    {
        var postsResult = await postsService.GetAllPosts(cancellationToken);
        if (postsResult.IsFailure)
        {
            Results.StatusCode((int)postsResult.Error!.StatusCode);
        }
        return postsResult.Value;
    })
    .WithName("GetPosts")
    .WithOpenApi()
    .WithTags("Posts");

app.MapGet("/posts/{id}", async ([FromRoute] int id, IPostsService postsService, CancellationToken cancellationToken) =>
    {
        var postsResult = await postsService.GetPostById(id, cancellationToken);
        if (postsResult.IsFailure)
        {
            Results.StatusCode((int)postsResult.Error!.StatusCode);
        }
        return postsResult.Value;
    })
    .WithName("GetPostsById")
    .WithOpenApi()
    .WithTags("Posts");


app.MapPost("/posts", async ([FromBody] PostRecord post, IPostsService postsService, CancellationToken cancellationToken) =>
    {
        var postResult = await postsService.CreatePost(post, cancellationToken);
        if (postResult.IsFailure)
        {
            Results.StatusCode((int)postResult.Error!.StatusCode);
        }
        return Results.Created($"posts/{postResult.Value!.Id}", postResult.Value);
    })
    .WithName("CreatePosts")
    .WithOpenApi()
    .WithTags("Posts");

app.MapDelete("/posts/{id}", async ([FromRoute] int id, IPostsService postsService, CancellationToken cancellationToken) =>
    {
        var deletePostResult = await postsService.DeletePostById(id, cancellationToken);
        if (deletePostResult.IsFailure)
        {
            Results.StatusCode((int)deletePostResult.Error!.StatusCode);
        }
        return Results.Ok();
    })
    .WithName("DeletePostsById")
    .WithOpenApi()
    .WithTags("Posts");

app.Run();