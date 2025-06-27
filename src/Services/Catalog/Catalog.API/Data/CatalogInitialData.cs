using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation)) return;

        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return
        [
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Wireless Mouse",
                Category = ["Electronics", "Computer Accessories"],
                Description = "Ergonomic wireless mouse with adjustable DPI.",
                ImageFile = "mouse.jpg",
                Price = 29.99m
            },

            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Stainless Steel Water Bottle",
                Category = ["Home", "Kitchen", "Outdoor"],
                Description = "Keeps drinks cold for 24 hours and hot for 12.",
                ImageFile = "bottle.jpg",
                Price = 19.95m
            },

            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Yoga Mat",
                Category = ["Fitness", "Health"],
                Description = "Eco-friendly yoga mat with non-slip surface.",
                ImageFile = "yogamat.jpg",
                Price = 35.50m
            }
        ];
    }
}