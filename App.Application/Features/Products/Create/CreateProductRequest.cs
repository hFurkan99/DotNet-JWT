namespace App.Application.Features.Products.Create
{
    public record CreateProductRequest(
        string Name,
        decimal Price,
        long UserId);
}