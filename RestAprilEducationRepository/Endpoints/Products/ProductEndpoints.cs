namespace RestAprilEducationRepository.API.Endpoints.Products
{
    public static class ProductEndpoints
    {
        // querystring =  /api/products?id=1  /api/product?pagesize=10&pageindex=1
        // route data =  /api/products/1      /api/products/pagesize/10/pageindex/1
        // body
        // header
        public static void AddProductEndpoints(this WebApplication app)
        {
            app.MapGroup("api/products")
                .AddGetAllProductEndpoint()
                .AddGetAllWithPagedProductEndpoint()
                .AddCreateProductEndpoint()
                .AddUpdateProductEndpoint()
                .AddDeleteProductEndpoint();
        }
    }
}
