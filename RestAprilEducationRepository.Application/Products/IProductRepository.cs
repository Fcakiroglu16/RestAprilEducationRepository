using System;
using System.Collections.Generic;
using System.Text;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Application.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> CreateAsync(Product product);

        Task<bool> AnyAsync(string productName);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> GetAllWithPagedAsync(int pageNumber, int pageSize);
    }
}
