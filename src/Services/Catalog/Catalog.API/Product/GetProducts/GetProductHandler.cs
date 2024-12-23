using BuildingBlocks.CORS;
using Marten;


namespace Catalog.API.Product.GetProducts
{
    public record GetProductQuery(): IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Catalog.API.Models.Product> Products);
    internal class GetProductQueryHandler
        (IDocumentSession session, ILogger<GetProductQueryHandler> logger)
        : IQueryHandler<GetProductQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductQueryHandlerHandle called with {@Query}",query);
            var products = await session.Query<Catalog.API.Models.Product>().ToListAsync(cancellationToken);
            return new GetProductsResult(products);

        }
    }
}
