using BuildingBlocks.CORS;
using Catalog.API.Exceptions;
using Catalog.API.Product.UpdateProduct;
using FluentValidation;
using Marten;

namespace Catalog.API.Product.UpdateProductById
{
    public record UpdateProductByIdCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductByIdResult>; 
    public record UpdateProductByIdResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required").Length(2, 250).WithMessage("Name must be between 2 and 250 character");
            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater then 0");
        }
    }
    public class UpdateProductByIdHandler(IDocumentSession session) : ICommandHandler<UpdateProductByIdCommand, UpdateProductByIdResult>
    {
        public async Task<UpdateProductByIdResult> Handle(UpdateProductByIdCommand command, CancellationToken cancellationToken)
        {
            

            var product = await session.LoadAsync<Models.Product>(command.Id,cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(command.Id);
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
