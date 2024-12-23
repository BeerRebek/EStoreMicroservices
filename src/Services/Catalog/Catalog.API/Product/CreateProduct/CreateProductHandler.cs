using BuildingBlocks.CORS;
using MediatR;
using System.ComponentModel;
using System.Windows.Input;
using Catalog.API.Models;
using Marten;
using FluentValidation;
using System.Linq;
namespace Catalog.API.Product.CreateProduct
{

    public record CreateProductCommand(string Name, List<string> Catogory, string Description,string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Catogory).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero required");
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession session,ILogger<CreateProductCommandHandler> logger ) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //Business Logic to creare new Product
            //create Product entity from command object
            //save to database
            //return CreateProjectResult result

            logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);

            var product = new Catalog.API.Models.Product
            {
                Name = command.Name,
                Category = command.Catogory,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };

            //save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);


            //return Result
            return new CreateProductResult(product.Id);
            
        }
    }
}
