using App.Application.Features.Products.Dto;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Products
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
