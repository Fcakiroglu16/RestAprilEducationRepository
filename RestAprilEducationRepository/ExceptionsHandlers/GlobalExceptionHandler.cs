#region

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace RestAprilEducationRepository.API.ExceptionsHandlers;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception, exception.Message);


        ProblemDetails problemDetails = new ProblemDetails
        {
            Title = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.",
            Status = StatusCodes.Status500InternalServerError,
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}