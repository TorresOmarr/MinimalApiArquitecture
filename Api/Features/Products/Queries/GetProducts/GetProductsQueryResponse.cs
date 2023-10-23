namespace Api.Features.Products.Queries.GetProducts;


public class GetProductsQueryResponse
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}