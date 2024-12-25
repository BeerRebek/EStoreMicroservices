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
        (IDocumentSession session) 
        :ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            

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
