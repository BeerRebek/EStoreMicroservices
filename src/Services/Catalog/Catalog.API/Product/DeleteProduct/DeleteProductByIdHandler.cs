using BuildingBlocks.CORS;
using Catalog.API.Exceptions;
using FluentValidation;
using Marten;

namespace Catalog.API.Product.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>; 
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
        }
    }
    internal class DeleteProductByIdCommandHandler
        (IDocumentSession session, ILogger<DeleteProductByIdCommandHandler> logger) 
        :ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("\"DeleteProductHandler Handle called with {@Command}", command);

            var product = await session.LoadAsync<Models.Product>(command.Id, cancellationToken); 
            if (product is null) 
            { 
                throw new ProductNotFoundException(command.Id); 
            }
            session.Delete(product); 
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
