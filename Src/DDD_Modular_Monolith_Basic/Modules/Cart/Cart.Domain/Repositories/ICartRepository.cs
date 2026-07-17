namespace Cart.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Cart entity data access operations.
    /// </summary>
    public interface ICartRepository
    {
        Task<IReadOnlyCollection<Entities.Cart>> GetAll(CancellationToken cancellationToken);
        Task<Entities.Cart?> GetById(Guid id, CancellationToken cancellationToken);
        Task Add(Entities.Cart cart, CancellationToken cancellationToken);
        Task Update(Entities.Cart cart, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
