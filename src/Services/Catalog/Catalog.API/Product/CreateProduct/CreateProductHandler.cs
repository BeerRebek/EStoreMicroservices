using BuildingBlocks.CORS;
using MediatR;
using System.ComponentModel;
using System.Windows.Input;
using Catalog.API.Models;
using Marten;
namespace Catalog.API.Product.CreateProduct
{

    public record CreateProductCommand(string Name, List<string> Catogory, string Description,string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //Business Logic to creare new Product
            //create Product entity from command object
            //save to database
            //return CreateProjectResult result

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
