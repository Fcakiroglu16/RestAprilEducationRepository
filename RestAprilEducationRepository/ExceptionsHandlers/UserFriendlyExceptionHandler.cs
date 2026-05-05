#region

using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.Domain.Exceptions;

#endregion

namespace RestAprilEducationRepository.API.ExceptionsHandlers;

public class UserFriendlyExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UserFriendlyException userFriendlyException) return false;
        ProblemDetails problemDetails = new ProblemDetails();


        problemDetails.Status = userFriendlyException.StatusCode ?? HttpStatusCode.BadRequest.GetHashCode();


        problemDetails.Title = exception.Message;


        httpContext.Response.StatusCode = HttpStatusCode.BadRequest.GetHashCode();

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


        return true;
    }
}