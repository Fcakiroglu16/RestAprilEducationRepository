using System;
using System.Collections.Generic;
using System.Text;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Application.Products
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllWithPagedAsync(int pageNumber, int pageSize);

        Task<Product?> AnyAsync(string name);
    }
}
