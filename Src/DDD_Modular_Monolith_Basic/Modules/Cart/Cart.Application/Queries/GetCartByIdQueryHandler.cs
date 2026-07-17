using Cart.Application.DTOs;
using Cart.Domain.Repositories;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cart.Application.Queries
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, Result<CartResponse>>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartByIdQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Result<CartResponse>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            //var cartId = CartId.Create(request.CartId);
            var cart = await _cartRepository.GetById(request.CartId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure<CartResponse>(CartErrors.NotFound(request.CartId));
            }

            return Result.Success(cart.ToResponse());
        }
    }

    public static class CartErrors
    {
        public static Error NotFound(Guid cartId) =>
            new("Cart.NotFound", $"Cart with ID '{cartId}' was not found.");
    }

}
