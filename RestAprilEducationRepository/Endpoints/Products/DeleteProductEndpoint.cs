using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application.Products;

namespace RestAprilEducationRepository.API.Endpoints.Products
{
    public static class DeleteProductEndpoints
    {
        // DELETE api/products/ahmet
        public static RouteGroupBuilder AddDeleteProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}",
                async ([FromRoute] int id,
                        [FromServices] IProductsApplication productsApplication) =>
                    (await productsApplication.DeleteAsync(id)).ToResult()).MapToApiVersion(1, 0);


            return group;
        }
    }
}
