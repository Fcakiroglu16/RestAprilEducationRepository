using Grpc.Core;
using Grpc.Core.Interceptors;

namespace GrpcServer;

public class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            // Bilinçli fırlatılan hatalar — olduğu gibi geç
            throw;
        }
        catch (Exception ex)
        {
            // Beklenmedik server hatası
            throw new RpcException(new Status(StatusCode.Internal, $"Sunucu hatası: {ex.Message}"));
        }
    }
}