using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel
{
    public interface IHasDomainEvent
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
