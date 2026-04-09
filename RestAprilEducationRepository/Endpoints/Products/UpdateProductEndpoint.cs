using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Application.Products.Update;

namespace RestAprilEducationRepository.API.Endpoints.Products
{
    public static class UpdateProductEndpoints
    {
        //PUT api/products/1 request body: { "name": "Updated Product", "price": 19.99 }


        public static RouteGroupBuilder AddUpdateProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}",
                async ([FromRoute] int id, [FromBody] UpdateProductRequest request,
                    [FromServices] IProductsApplication productsApplication) =>
                {
                    (await productsApplication.Update(id, request)).ToResult();
                });

            return group;
        }
    }
}
