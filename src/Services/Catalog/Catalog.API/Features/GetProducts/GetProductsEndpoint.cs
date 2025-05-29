using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.GetProducts;

public record GetProductsRequest(
    int PageNumber = 1,
    int PageSize = 10
);
public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromServices] ISender sender) =>
        {
            var query = new GetProductsQuery(pageNumber, pageSize);
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products List")
        .WithDescription("Get Products List");
    }
}