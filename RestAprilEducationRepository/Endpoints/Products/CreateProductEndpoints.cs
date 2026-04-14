using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Application.Products.Create;

namespace RestAprilEducationRepository.API.Endpoints.Products
{
    public static class CreateProductEndpoints
    {
        public static RouteGroupBuilder AddCreateProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/",
                    async ([FromBody] CreateProductRequest request,
                            [FromServices] IProductsApplication productsApplication) =>
                        (await productsApplication.Create(request)).ToResult())
                .AddEndpointFilter<ValidationFilter<CreateProductRequest>>().MapToApiVersion(1, 0);


            return group;
        }
    }
}
