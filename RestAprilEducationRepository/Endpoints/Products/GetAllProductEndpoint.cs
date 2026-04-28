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
                (await productsApplication.GetAllAsync()).ToResult()).MapToApiVersion(1, 0);

            return group;
        }
    }
}
