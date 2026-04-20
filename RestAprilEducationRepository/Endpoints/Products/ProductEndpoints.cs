using Asp.Versioning.Builder;
using RestAprilEducationRepository.API.Endpoints.Products.Create.V1;
using RestAprilEducationRepository.API.Endpoints.Products.Create.V2;

namespace RestAprilEducationRepository.API.Endpoints.Products
{
    public static class ProductEndpoints
    {
        // querystring =  /api/products?id=1  /api/product?pagesize=10&pageindex=1
        // route data =  /api/products/1      /api/products/pagesize/10/pageindex/1
        // body
        // header
        public static void AddProductEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            //route constraint => int, bool, datetime, decimal, double, float, guid
            // api/v1/products
            app.MapGroup("api/v{version:apiVersion}/products").WithApiVersionSet(apiVersionSet)
                .AddGetAllProductEndpoint()
                .AddGetAllWithPagedProductEndpoint()
                .AddCreateProductEndpoint()
                .AddCreateProductV2Endpoint()
                .AddUpdateProductEndpoint()
                .AddDeleteProductEndpoint();
        }
    }
}
