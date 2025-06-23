﻿namespace Catalog.API.Features.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required!");
        RuleFor(p => p.Category).NotEmpty().WithMessage("Category is required!");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required!");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

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