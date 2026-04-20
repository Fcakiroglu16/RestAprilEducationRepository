using RestAprilEducationRepository.Domain.Exceptions;

namespace RestAprilEducationRepository.API.Endpoints.ExceptionHandlerExamples
{
    public static class ExceptionHandlerExampleEndpoints
    {
        public static void AddExceptionHandlerExampleEndpoint(this WebApplication app)
        {
            app.MapPost("/api/exception-handler-example", () =>
            {
                //throw new Exception("veri tabanı bağlantı hatası");

                throw new UserFriendlyException("user friendly exception");
                //throw new BusinessException("business exception");
                return Results.Ok("ok");
            });
        }
    }
}
