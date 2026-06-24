using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.DTOs
{
    
    public sealed record CategoryInfo(Guid? CategoryId, string Name, Photo? Image);

    public sealed record ProductInfo(Guid? ProductId, string Name, string Description, decimal Price, Photo? Image, CategoryId CategoryId, string CategoryName);
}
