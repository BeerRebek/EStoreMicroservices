using BuildingBlocks.CORS;
using Marten;
using Marten.Linq.QueryHandlers;
using System.Linq;

namespace Catalog.API.Product.GetProductByCatogory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Models.Product> Products);
    public class GetProductByCategoryQueryHandler(IDocumentSession session,ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByIdQueryHandler.Handle called {@Query}", query);

            var products = await session.Query<Models.Product>().Where(p=>p.Category.Contains(query.Category)).ToListAsync();
            return new GetProductByCategoryResult(products);
        }
    }
}
