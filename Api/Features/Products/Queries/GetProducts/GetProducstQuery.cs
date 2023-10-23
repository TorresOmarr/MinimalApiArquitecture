
using MediatR;

namespace Api.Features.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<List<GetProductsQueryResponse>>
{
}
