using BuildingBlocks.CORS;
using Catalog.API.Exceptions;
using Marten;

namespace Catalog.API.Product.UpdateProductById
{
    public record UpdateProductByIdCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductByIdResult>; 
    public record UpdateProductByIdResult(bool IsSuccess);
    public class UpdateProductByIdHandler(IDocumentSession session, ILogger logger) : ICommandHandler<UpdateProductByIdCommand, UpdateProductByIdResult>
    {
        public async Task<UpdateProductByIdResult> Handle(UpdateProductByIdCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductByIdHandler Handle called with {@Command}", command);

            var product = await session.LoadAsync<Models.Product>(command.Id,cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }
            product.Name = command.Name; 
            product.Description = command.Description; 
            product.Price = command.Price; 
            product.ImageFile = command.ImageFile; 
            product.Category = command.Category; 
            
            session.Update(product); 
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductByIdResult(true);
        }
    }
}
