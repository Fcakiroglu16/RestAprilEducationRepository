using Grpc.Core;
using Scalar.AspNetCore;
using Todo.RestClient.Protos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddGrpcClient<TodoService.TodoServiceClient>(x => { x.Address = new Uri("https://localhost:7177"); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


// ── CREATE ── POST /api/todos
app.MapPost("/api/todos", async (CreateTodoRequest request, TodoService.TodoServiceClient client) =>
{
    var todo = await client.CreateTodoAsync(request);
    return Results.Created($"/api/todos/{todo.Id}", todo);
});

// ── DELETE ── DELETE /api/todos/{id}
app.MapDelete("/api/todos/{id:int}", async (int id, TodoService.TodoServiceClient client) =>
{
    var response = await client.DeleteTodoAsync(new DeleteTodoRequest { Id = id });
    return response.Success ? Results.NoContent() : Results.NotFound();
});

// ── LIST (Server Streaming) ── GET /api/todos
app.MapGet("/api/todos", async (TodoService.TodoServiceClient client) =>
{
    var todos = new List<TodoItem>();
    using var streamingCall = client.ListTodos(new ListTodosRequest());


    await foreach (var item in streamingCall.ResponseStream.ReadAllAsync())
    {
        todos.Add(item);
    }

    return Results.Ok(todos);
});

// ── BULK CREATE (Client Streaming) ── POST /api/todos/bulk
app.MapPost("/api/todos/bulk", async (List<CreateTodoRequest> requests, TodoService.TodoServiceClient client) =>
{
    using var streamingCall = client.BulkCreateTodos();
    foreach (var request in requests)
    {
        await streamingCall.RequestStream.WriteAsync(request);
    }

    await streamingCall.RequestStream.CompleteAsync();
    var response = await streamingCall.ResponseAsync;
    return Results.Ok(response);
});

// ── UPDATE (Bidirectional Streaming) ── PUT /api/todos
app.MapPut("/api/todos", async (List<UpdateTodoRequest> requests, TodoService.TodoServiceClient client) =>
{
    var updatedItems = new List<TodoItem>();
    using var streamingCall = client.UpdateTodos();

    var readTask = Task.Run(async () =>
    {
        await foreach (var item in streamingCall.ResponseStream.ReadAllAsync())
        {
            updatedItems.Add(item);
        }
    });

    foreach (var request in requests)
    {
        await streamingCall.RequestStream.WriteAsync(request);
    }

    await streamingCall.RequestStream.CompleteAsync();

    await readTask;

    return Results.Ok(updatedItems);
});


app.Run();

