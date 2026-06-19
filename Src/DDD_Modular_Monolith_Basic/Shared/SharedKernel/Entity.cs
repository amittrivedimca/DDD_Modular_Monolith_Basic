using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel
{
    /// <summary>
    /// Base class for all entities in the domain.
    /// An entity is an object that has a distinct identity that runs through time and different states.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public abstract class Entity<TId> where TId : notnull
    {
        /// <summary>
        /// The unique identifier for this entity.
        /// </summary>
        public TId Id { get; protected init; } = default!;

        /// <summary>
        /// Determines whether the specified object is equal to the current entity.
        /// Two entities are equal if they have the same identifier.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;
            if (obj is not Entity<TId> entity) return false;

            return Id.Equals(entity.Id);
        }

        /// <summary>
        /// Returns the hash code for this entity based on its identifier.
        /// </summary>
        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
    }
}
