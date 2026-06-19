using SharedKernel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Text;

namespace Catalog.Domain.ValueObjects
{
    public class Photo : ValueObject
    {
        public string Name { get; set; }
        public string Url { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Url;
        }
    }
}
