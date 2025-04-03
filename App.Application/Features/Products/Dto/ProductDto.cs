namespace App.Application.Features.Products.Dto
{
    public record ProductDto(long Id, string Name, Decimal Price, long UserId);
}
