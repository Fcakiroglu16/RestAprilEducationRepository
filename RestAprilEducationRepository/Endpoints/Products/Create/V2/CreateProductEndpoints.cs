using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Application.Products.Create;

namespace RestAprilEducationRepository.API.Endpoints.Products.Create.V2
{
    public static class FilterExampleEndpoint
    {
        public static RouteGroupBuilder AddCreateProductV2Endpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/",
                    async ([FromBody] CreateProductRequest request,
                            [FromServices] IProductsApplication productsApplication) =>
                        Results.Ok("AddCreateProductV2Endpoint"))
                .AddEndpointFilter<ValidationFilter<CreateProductRequest>>().MapToApiVersion(2, 0);


            return group;
        }
    }
}
