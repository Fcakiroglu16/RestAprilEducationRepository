using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Application.Products.Create;

namespace RestAprilEducationRepository.API.Endpoints.Products
{
    public static class FilterExampleEndpoints
    {
        public static RouteGroupBuilder AddFilterEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/", () => Results.Ok("filter endpoint"))
                .AddEndpointFilter(async (context, next) =>
                {
                    //1.filter before(1)

                    var response = await next(context);

                    //1.filter after(4)

                    return response;
                })
                .AddEndpointFilter(async (context, next) =>
                {
                    //2.filter before(1)

                    var response = await next(context);

                    //1.filter after(3)

                    return response;
                });


            return group;
        }
    }
}
