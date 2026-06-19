using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.DTOs
{
    
    public sealed record CategoryInfo(Guid CategoryId, string Name);
}
