using System.Net;
using RestAprilEducationRepository.Application;

namespace RestAprilEducationRepository.API.Extensions
{
    public static class EndpointExtensions
    {
        //Created => response body , header   api/products/5
        public static IResult ToResult(this ApplicationResult applicationResult)
        {
            if (applicationResult.IsSuccess)
            {
                return applicationResult switch
                {
                    { HttpStatusCode: HttpStatusCode.Created } => Results.Created(string.Empty, null),
                    { HttpStatusCode: HttpStatusCode.NoContent } => Results.NoContent(),
                };
            }

            return Results.Problem(applicationResult.Problem!);
        }

        public static IResult ToResult<T>(this ApplicationResult<T> applicationResult)
        {
            if (applicationResult.IsSuccess)
            {
                return applicationResult switch
                {
                    { HttpStatusCode: HttpStatusCode.Created } => Results.Created(string.Empty, applicationResult.Data),
                    _ => Results.Ok(applicationResult.Data)
                };
            }

            return Results.Problem(applicationResult.Problem!);
        }
    }
}
