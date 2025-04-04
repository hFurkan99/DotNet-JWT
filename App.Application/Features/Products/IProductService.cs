using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;

namespace App.Application.Features.Products
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<ProductDto>>> GetAllAsync();
        Task<ServiceResult<long>> CreateAsync(CreateProductRequest request);
        Task<ServiceResult> UpdateAsync(UpdateProductRequest request);
        Task<ServiceResult> DeleteAsync(int id);

    }
}
