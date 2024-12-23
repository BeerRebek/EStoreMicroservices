using BuildingBlocks.CORS;
using Catalog.API.Exceptions;
using Catalog.API.Product.UpdateProductById;
using FluentValidation;
using Marten;
using System.Windows.Input;

namespace Catalog.API.Product.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Catogory, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required").Length(2, 250).WithMessage("Name must be between 2 and 250 character");
            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater then 0");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
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