using System;
using System.Collections.Generic;
using System.Text;

namespace Cart.Application.DTOs
{
    public sealed record CartResponse(Guid CartId, Guid? UserId, List<CartItemResponse> Items);

    public sealed record CartItemResponse(Guid ProductId, string Name, string Description, decimal SellPrice, int Quantity);
}
