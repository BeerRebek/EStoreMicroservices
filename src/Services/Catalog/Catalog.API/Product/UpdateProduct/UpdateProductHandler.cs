using BuildingBlocks.CORS;
using Catalog.API.Exceptions;
using Marten;
using System.Windows.Input;

namespace Catalog.API.Product.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Catogory, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandHandler Handle called with {@Command}", command);

            var product = await session.LoadAsync<Models.Product>(command.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }
            product.Name=command.Name;
            product.Description=command.Description;
            product.Price=command.Price;
            product.ImageFile=command.ImageFile;
            product.Category = command.Catogory;


            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}