using App.Application.Contracts.Persistence;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Domain.Entities;
using AutoMapper;
using System.Net;

namespace App.Application.Features.Products
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<Product, long> productRepository) : IProductService
    {
        public async Task<ServiceResult<long>> CreateAsync(CreateProductRequest request)
        {
            var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name);

            if (isProductNameExist)
                return ServiceResult<long>.Fail("Ürün ismi veritabanında bulunmaktadır.");

            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();


            return ServiceResult<long>.SuccessAsCreated(product.Id, $"/api/Products/GetById?id={product.Id}");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            productRepository.Delete(product!);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<IEnumerable<ProductDto>>> GetAllAsync()
        {
            var products = await productRepository.GetAllAsync();

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<IEnumerable<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.Id);
            if (product is null)
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);

            var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id != product.Id);

            if (isProductNameExist)
                return ServiceResult.Fail("Ürün ismi veritabanında bulunmaktadır.");

            mapper.Map(request, product);

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
