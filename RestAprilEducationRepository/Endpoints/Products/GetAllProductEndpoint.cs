using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application.Products;

namespace RestAprilEducationRepository.API.Endpoints.Products
{
    public static class GetAllProductEndpoints
    {
        // GET api/products
        // Get api/products?pageNumber=1&pageSize=10
        // Get api/products/1/10
        public static RouteGroupBuilder AddGetAllProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/",
                async ([FromServices] IProductsApplication productsApplication) =>
                    (await productsApplication.GetAll()).ToResult());

            return group;
        }
    }

    public static class GetAllWithPagedProductEndpoints
    {
        // GET api/products
        public static RouteGroupBuilder AddGetAllWithPagedProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/{pageNumber}/{pageSize}",
                async ([FromRoute] int pageNumber, [FromRoute] int pageSize,
                        [FromServices] IProductsApplication productsApplication) =>
                    (await productsApplication.GetAll()).ToResult());

            return group;
        }
    }
}
