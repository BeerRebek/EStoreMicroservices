using BuildingBlocks.CORS;
using Catalog.API.Exceptions;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Product.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Models.Product Product);
    public class GetProductByIdQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            

            var product = await session.LoadAsync<Models.Product>(query.Id,cancellationToken);

            if(product is null)
            {
                throw new ProductNotFoundException(query.Id);
            }
            return new GetProductByIdResult(product);
        }
    }
}
