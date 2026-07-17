using Cart.Application.DTOs;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cart.Application.Queries
{    
    public sealed record GetCartByIdQuery(Guid CartId) : IRequest<Result<CartResponse>>;
}
