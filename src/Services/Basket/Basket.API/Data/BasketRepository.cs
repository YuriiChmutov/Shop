using Basket.API.Exceptions;

namespace Basket.API.Data;

public class BasketRepository(IQuerySession qSession, IDocumentSession dSession) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await qSession.LoadAsync<ShoppingCart>(userName, cancellationToken);
        return basket ?? throw new BasketNotFoundException(userName);
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        dSession.Store(basket);
        await dSession.SaveChangesAsync(cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        dSession.Delete<ShoppingCart>(userName);
        await dSession.SaveChangesAsync(cancellationToken);
        return true;
    }
}