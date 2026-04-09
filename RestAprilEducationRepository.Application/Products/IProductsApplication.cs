using RestAprilEducationRepository.Application.Products.Create;
using RestAprilEducationRepository.Application.Products.GetList;
using RestAprilEducationRepository.Application.Products.Update;

namespace RestAprilEducationRepository.Application.Products;

public interface IProductsApplication
{
    Task<ApplicationResult<List<ProductDto>>> GetAll();
    Task<ApplicationResult<CreateProductResponse>> Create(CreateProductRequest request);
    Task<ApplicationResult> Update(int id, UpdateProductRequest request);
    Task<ApplicationResult> Delete(int id);

    Task<ApplicationResult<List<ProductDto>>> GetAllWithPaged(int pageNumber, int pageSize);
}
