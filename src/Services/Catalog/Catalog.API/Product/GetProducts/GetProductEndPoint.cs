﻿using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Product.GetProducts
{
    //public record GetProductsRequest()
    public record GetProductsResponse(IEnumerable<Models.Product>Products);
    public class GetProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductQuery());
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("GetProducts")
            .WithDescription("GetProducts");
        }
    }
}
