namespace Catalog.API.Features.GetProducts;

public record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IQuery<GetProductsResult>;

public record GetProductsResult(
    IEnumerable<Product> Products
);

public class GetProductsQueryHandler(IQuerySession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // get data from db
        var products = await session.Query<Product>()
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new GetProductsResult(products);
    }
}