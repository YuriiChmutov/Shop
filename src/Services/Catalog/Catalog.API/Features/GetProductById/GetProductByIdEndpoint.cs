using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Features.GetProductById;

public record GetProductByIdRequest();

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async ([FromRoute] string id, [FromServices] ISender sender) =>
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return Results.BadRequest("Invalid GUID format");
            }
            var result = await sender.Send(new GetProductByIdQuery(guid));
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}