using RestAprilEducationRepository.Application.Products.Create;
using RestAprilEducationRepository.Application.Products.GetList;
using RestAprilEducationRepository.Application.Products.Update;

namespace RestAprilEducationRepository.Application.Products;

public interface IProductsApplication
{
    Task<ApplicationResult<List<ProductDto>>> GetAllAsync();
    Task<ApplicationResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);
    Task<ApplicationResult> UpdateAsync(int id, UpdateProductRequest request);
    Task<ApplicationResult> DeleteAsync(int id);

    Task<ApplicationResult<List<ProductDto>>> GetAllWithPagedAsync(int pageNumber, int pageSize);
}
