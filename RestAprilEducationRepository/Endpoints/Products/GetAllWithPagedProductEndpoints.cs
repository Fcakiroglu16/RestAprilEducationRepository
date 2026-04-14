using Microsoft.AspNetCore.Mvc;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application.Products;

namespace RestAprilEducationRepository.API.Endpoints.Products;

public static class GetAllWithPagedProductEndpoints
{
    // GET api/products
    public static RouteGroupBuilder AddGetAllWithPagedProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{pageNumber:int}/{pageSize:int}",
            async ([FromRoute] int pageNumber, [FromRoute] int pageSize,
                    [FromServices] IProductsApplication productsApplication) =>
                (await productsApplication.GetAll()).ToResult()).MapToApiVersion(1, 0);

        return group;
    }
}