using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application.Products;

namespace RestAprilEducationRepository.API.Endpoints.Products
{
    namespace RestAprilEducationRepository.API.Endpoints.Products
    {
        public static class GetAllProductEndpoints2
        {

            // POST  api/comments => request body => { "productId": 1, "comment": "This is a comment." }
            // POST api/products/1/comments 
            // Get api/products/1/comments =>  products + comments list
            // GET api/products
            // Get api/products/paged?pageNumber=1&pageSize=10
            // Get api/products/pagenumber/1/10
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
                group.MapGet("/paged",
                    async ([FromQuery] int pageNumber, [FromQuery] int pageSize,
                            [FromServices] IProductsApplication productsApplication) =>
                        (await productsApplication.GetAll()).ToResult());

                return group;
            }
        }
    }
}
