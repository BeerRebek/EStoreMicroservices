
using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Product.UpdateProductById
{
    public record UpdateProductByIdRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price); 
    public record UpdateProductByIdResponse(bool IsSuccess);
    public class UpdatetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductByIdRequest request, ISender sender) =>
            {
                var command = new UpdateProductByIdCommand(id, request.Name, request.Category, request.Description, request.ImageFile, request.Price); var result = await sender.Send(command);

                var response = result.Adapt<UpdateProductByIdResponse>();

                return Results.Ok(response);
            })
                .WithName("Update Product By ID")
                .Produces<UpdateProductByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Update Product By ID")
                .WithDescription("Update the details of a product by its ID");
        }
    }
}
