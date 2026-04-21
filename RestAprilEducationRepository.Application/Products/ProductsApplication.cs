using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestAprilEducationRepository.Application.Products.Create;
using RestAprilEducationRepository.Application.Products.GetList;
using RestAprilEducationRepository.Application.Products.Update;
using RestAprilEducationRepository.Domain;

namespace RestAprilEducationRepository.Application.Products
{
    public class ProductsApplication(
        IProductRepository productRepository,
        ILogger<ProductsApplication> logger,
        ILoggerFactory loggerFactory) : IProductsApplication
    {
        public async Task<ApplicationResult<List<ProductDto>>> GetAll()
        {
            //trace
            //debug
            // information
            //warning
            // error
            // critical


            logger.LogInformation("GetAll methodu çalıştı");

            var loggerFromFactory = loggerFactory.CreateLogger("ProductsApplicationCategoryName");

            loggerFromFactory.LogInformation("GetAll methodu çalıştı 2");


            var productList = await productRepository.GetAllAsync();

            var productsAsDto = productList.Select(product =>
                new ProductDto(product.Id, product.Name, product.Price + product.Price * 0.20m)).ToList();


            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetAllWithPaged(int pageNumber, int pageSize)
        {
            var productList = await productRepository.GetAllWithPagedAsync(pageNumber, pageSize);

            var productsAsDto = productList.Select(product =>
                new ProductDto(product.Id, product.Name, product.Price + product.Price * 0.20m)).ToList();


            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }


        public async Task<ApplicationResult<CreateProductResponse>> Create(CreateProductRequest request)
        {
            var hasProduct = await productRepository.AnyAsync(request.Name);

            //Result Pattern => Success, Failure

            if (hasProduct)
            {
                return ApplicationResult<CreateProductResponse>.Failure("Product with the same name already exists.",
                    HttpStatusCode.BadRequest);
            }


            var barcode = Guid.NewGuid().ToString();

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Barcode = barcode
            };

            var createdProduct = await productRepository.CreateAsync(product);


            return ApplicationResult<CreateProductResponse>.Success(new CreateProductResponse(createdProduct.Id));
        }

        public async Task<ApplicationResult> Update(int id, UpdateProductRequest request)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ApplicationResult.Failure("Product not found.", HttpStatusCode.NotFound);
            }

            var hasProductWithSameName = await productRepository.AnyAsync(request.Name);

            if (hasProductWithSameName && product.Name != request.Name)
            {
                return ApplicationResult.Failure("Product with the same name already exists.",
                    HttpStatusCode.BadRequest);
            }

            product.Name = request.Name;
            product.Price = request.Price;

            await productRepository.UpdateAsync(product);

            return ApplicationResult.Success();
        }

        public async Task<ApplicationResult> Delete(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ApplicationResult.Failure("Product not found.", HttpStatusCode.NotFound);
            }

            await productRepository.DeleteAsync(id);

            return ApplicationResult.Success();
        }
    }
}
