using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel
{
    public sealed record Error(string Code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "A null value was provided.");

        public static implicit operator string(Error error) => error.Code;
    }
}
