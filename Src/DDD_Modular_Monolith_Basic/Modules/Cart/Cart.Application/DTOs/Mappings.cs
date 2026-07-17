using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cart.Application.DTOs
{
    public static class Mappings
    {
        public static CartResponse ToResponse(this Domain.Entities.Cart cart)
        {
            return new CartResponse(cart.Id.Id,
                cart.UserId,
                cart.CartItems.Select(item => new CartItemResponse
                (
                    item.Id.Id,
                    item.Name,
                    string.Empty, //item.Description,
                    item.SellPrice,
                    item.Quantity                    
                )).ToList()
            );
        }
    }
}
