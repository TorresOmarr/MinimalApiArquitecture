using MediatR;

namespace Api.Features.Products.Queries.GetProduct;
public class GetProductByIdQuery : IRequest<GetProductByIdQueryResponse>
{
    public int ProductId { get; set; }
}


