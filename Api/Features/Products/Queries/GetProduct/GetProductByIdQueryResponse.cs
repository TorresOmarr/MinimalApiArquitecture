namespace Api.Features.Products.Queries.GetProduct;
public class GetProductByIdQueryResponse
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}