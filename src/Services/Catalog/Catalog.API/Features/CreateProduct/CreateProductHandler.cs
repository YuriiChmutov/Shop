namespace Catalog.API.Features.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create product from command
        var product = command.Adapt<Product>();

        // save to db
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // return product's id
        return new CreateProductResult(product.Id);
    }
}