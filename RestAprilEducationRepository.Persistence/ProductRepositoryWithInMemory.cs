using System;
using System.Collections.Generic;
using System.Text;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Persistence
{
    public class ProductRepositoryWithInMemory : IProductRepository
    {
        public Task<List<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> CreateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(string productName)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllWithPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
