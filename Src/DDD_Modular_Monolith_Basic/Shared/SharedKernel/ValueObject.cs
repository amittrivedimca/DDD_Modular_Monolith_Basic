namespace SharedKernel
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Gets the components that determine equality for this value object.
        /// </summary>
        /// <returns>An enumeration of objects that make up the value object's identity.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;
            if (obj is not ValueObject valueObject) return false;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public bool Equals(ValueObject? other)
        {
            return Equals((object?)other);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
    }
}
