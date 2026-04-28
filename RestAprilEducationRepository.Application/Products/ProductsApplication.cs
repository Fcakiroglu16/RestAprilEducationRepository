using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
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
        ILoggerFactory loggerFactory,
        IUnitOfWork unitOfWork) : IProductsApplication
    {
        public async Task<ApplicationResult<List<ProductDto>>> GetAllAsync()
        {
            logger.LogInformation("GetAll methodu çalıştı");

            var loggerFromFactory = loggerFactory.CreateLogger("ProductsApplicationCategoryName");

            loggerFromFactory.LogInformation("GetAll methodu çalıştı 2");


            var productList = await productRepository.GetAllAsync();

            var productsAsDto = productList.Select(product =>
                new ProductDto(product.Id, product.Name, product.Price + product.Price * 0.20m)).ToList();


            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetAllWithPagedAsync(int pageNumber, int pageSize)
        {
            var productList = await productRepository.GetAllWithPagedAsync(pageNumber, pageSize);

            var productsAsDto = productList.Select(product =>
                new ProductDto(product.Id, product.Name, product.Price + product.Price * 0.20m)).ToList();


            return ApplicationResult<List<ProductDto>>.Success(productsAsDto);
        }


        public async Task<ApplicationResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            //Task(Reference type -heap )  ValueTask(value type -stack) => Async işlemlerde kullanılır. Task, ValueTask'ten daha ağırdır. ValueTask, performans kritik durumlarda tercih edilir.

            var hasProduct = await productRepository.AnyAsync(request.Name);


            //var result = await Task.WhenAll(hasProduct, hasProduct2, hasProduct3);


            //Result Pattern => Success, Failure

            if (hasProduct is not null)
            {
                return ApplicationResult<CreateProductResponse>.Failure("Product with the same name already exists.",
                    HttpStatusCode.BadRequest);
            }


            var barcode = Guid.NewGuid().ToString();

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Barcode = barcode,
                CategoryId = request.CategoryId
            };
            await productRepository.AddAsync(product);


            await unitOfWork.CommitAsync();

            return ApplicationResult<CreateProductResponse>.Success(new CreateProductResponse(product.Id));
        }

        public async Task<ApplicationResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ApplicationResult.Failure("Product not found.", HttpStatusCode.NotFound);
            }

            var hasProductWithSameName = await productRepository.AnyAsync(request.Name);

            if (hasProductWithSameName != null && product.Name != request.Name)
            {
                return ApplicationResult.Failure("Product with the same name already exists.",
                    HttpStatusCode.BadRequest);
            }

            product.Name = request.Name;
            product.Price = request.Price;

            await productRepository.UpdateAsync(product);
            await unitOfWork.CommitAsync();
            return ApplicationResult.Success();
        }

        public async Task<ApplicationResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ApplicationResult.Failure("Product not found.", HttpStatusCode.NotFound);
            }

            await productRepository.DeleteAsync(product);
            await unitOfWork.CommitAsync();
            return ApplicationResult.Success();
        }
    }
}
