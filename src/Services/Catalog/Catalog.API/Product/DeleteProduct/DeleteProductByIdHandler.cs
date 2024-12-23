using BuildingBlocks.CORS;
using Catalog.API.Exceptions;
using Marten;

namespace Catalog.API.Product.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>; 
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductByIdCommandHandler
        (IDocumentSession session, ILogger logger) 
        :ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("\"DeleteProductHandler Handle called with {@Command}", command);

            var product = await session.LoadAsync<Models.Product>(command.Id, cancellationToken); 
            if (product is null) 
            { 
                throw new ProductNotFoundException(); 
            }
            session.Delete(product); 
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
