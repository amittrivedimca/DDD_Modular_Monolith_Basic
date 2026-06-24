using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cart.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<IReadOnlyCollection<Entities.Cart>> GetAll();
        Task<Entities.Cart?> GetById(Guid id);
        Task Add(Entities.Cart cart);
        Task Update(Entities.Cart cart);
        Task Delete(Guid id);
    }
}
