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

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("Product ID is required");

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required!")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var dbProduct = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (dbProduct == null) throw new ProductNotFoundException(command.Id);

        try
        {
            dbProduct.Name = command.Name;
            dbProduct.Category = [.. command.Category];
            dbProduct.Description = command.Description;
            dbProduct.ImageFile = command.ImageFile;
            dbProduct.Price = command.Price;

            session.Update(dbProduct);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
        catch
        {
            return new UpdateProductResult(false);
        }
    }
}