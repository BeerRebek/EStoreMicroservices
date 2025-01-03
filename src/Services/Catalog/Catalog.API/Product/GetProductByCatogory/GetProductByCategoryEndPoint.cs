﻿using Carter;
using Catalog.API.Product.CreateProduct;
using Mapster;
using MediatR;

namespace Catalog.API.Product.GetProductByCatogory
{

    //public record GetProductByCategoryRequest();
    public record GetProductByCategoryResponse(IEnumerable<Models.Product> Products);
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/category/{category}", 
                async (string category, ISender sender) =>
                {
                    var result = await sender.Send(new GetProductByCategoryQuery(category));
                    var response = result.Adapt<GetProductByCategoryResponse>();
                    return Results.Ok(response);
                })
                .WithName("GetProductByCategory")
                .Produces<GetProductByCategoryResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product By Category")
                .WithDescription("Get Product By Category"); 
        }
    }
}
