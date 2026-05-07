using System.Reflection.Metadata.Ecma335;
using Google.Protobuf;
using Grpc.Core;
using GrpcServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Todo.GrpcServer.Protos;
using TodoItem = Todo.GrpcServer.Protos.TodoItem;

namespace GrpcServer.Services
{
    public class TodoService(AppDbContext dbContext) : Todo.GrpcServer.Protos.TodoService.TodoServiceBase
    {
        public override async Task<TodoItem> CreateTodo(CreateTodoRequest request, ServerCallContext context)
        {
            var todo = new GrpcServer.Repositories.TodoItem
            {
                Title = request.Title
            };

            dbContext.TodoItems.Add(todo);

            await dbContext.SaveChangesAsync();


            return new TodoItem() { Id = todo.Id, IsCompleted = todo.IsCompleted, Title = todo.Title };
        }

        public override async Task<DeleteTodoResponse> DeleteTodo(DeleteTodoRequest request, ServerCallContext context)
        {
            var todo = await dbContext.TodoItems.FindAsync(request.Id);

            if (todo is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Todo {request.Id} bulunamadı."));

            dbContext.TodoItems.Remove(todo);
            await dbContext.SaveChangesAsync();

            return new DeleteTodoResponse { Success = true };
        }


        public override async Task ListTodos(ListTodosRequest request, IServerStreamWriter<TodoItem> responseStream,
            ServerCallContext context)
        {
            await foreach (var todo in dbContext.TodoItems
                               .AsNoTracking()
                               .AsAsyncEnumerable()
                               .WithCancellation(context.CancellationToken))
            {
                await responseStream.WriteAsync(new TodoItem
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    IsCompleted = todo.IsCompleted
                }, context.CancellationToken);
            }
        }


        public override async Task<BulkCreateResponse> BulkCreateTodos(
            IAsyncStreamReader<CreateTodoRequest> requestStream,
            ServerCallContext context)
        {
            var entities = new List<GrpcServer.Repositories.TodoItem>();

            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                entities.Add(new GrpcServer.Repositories.TodoItem
                {
                    Title = request.Title,
                    IsCompleted = false
                });
            }

            await dbContext.TodoItems.AddRangeAsync(entities, context.CancellationToken);
            await dbContext.SaveChangesAsync(context.CancellationToken);

            var response = new BulkCreateResponse { CreatedCount = entities.Count };
            response.Items.AddRange(entities.Select(e => new TodoItem
            {
                Id = e.Id,
                Title = e.Title,
                IsCompleted = e.IsCompleted
            }));

            return response;
        }


        public override async Task UpdateTodos(IAsyncStreamReader<UpdateTodoRequest> requestStream,
            IServerStreamWriter<TodoItem> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                var todo = await dbContext.TodoItems.FindAsync([request.Id], context.CancellationToken);

                if (todo is null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Todo {request.Id} bulunamadı."));
                }

                todo.Title = request.Title;
                todo.IsCompleted = request.IsCompleted;

                await dbContext.SaveChangesAsync(context.CancellationToken);

                await responseStream.WriteAsync(new TodoItem
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    IsCompleted = todo.IsCompleted
                }, context.CancellationToken);
            }
        }
    }
}
