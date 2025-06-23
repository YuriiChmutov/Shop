namespace Catalog.API.Features.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductHandler.Handle called with {@Command}", command);

        var dbProduct = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (dbProduct == null) throw new ProductNotFoundException();

        try
        {
            dbProduct.Name = command.Name;
            dbProduct.Category = [.. command.Category];
            dbProduct.Description = command.Description;
            dbProduct.ImageFile = command.ImageFile;
            dbProduct.Price = command.Price;

            session.Update(dbProduct);
            await session.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"Product was successfully updated.");
            return new UpdateProductResult(true);
        }
        catch (Exception e)
        {
            logger.LogError($"Failed to update product data: {e.Message}");
            return new UpdateProductResult(false);
        }
    }
}