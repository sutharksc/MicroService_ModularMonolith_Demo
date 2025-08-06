using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OrderModule.Application.Abstractions;
using SharedModule.Booking.Grpc;
using SharedModule.EndPoint;
using SharedModule.Protos;
using SharedModule.Shared;

namespace OrderModule.Application.Features.Order
{
    public class GetOrderByIdResult
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        public List<OrderEventResult> Events { get; set; }

        public UserResponse? User { get; set; }
        public BookingResponse? Booking { get; set; }
    }

    public class OrderEventResult
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EventType { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public decimal Amount { get; set; }
    }



    public class GetOrderByIdQuery : IRequest<Result>
    {
        public Guid OrderId { get; set; }
    }

    public class GetOrdersByIdHandler(IOrderService orderService) : IRequestHandler<GetOrderByIdQuery, Result>
    {
        public async Task<Result> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderByIdAsync(query.OrderId, cancellationToken);
            if (order == null) return Result.Failure(ClientErrors.OrderNotFound);

            return Result.Success(order);
        }
    }

    public class GetOrderByIdEndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapPost("api/order/get-by-id", async (GetOrderByIdQuery query, IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(query));
            }).RequireAuthorization();
        }
    }
}
