using BookingModule.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedModule.EndPoint;
using SharedModule.Shared;
using System.Net;

namespace BookingModule.Application.Features.Booking
{
    public class CreateBookingAndOrdersCommand : IRequest<Result>
    {
        public DateTime BookingDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }

        public List<CreateOrderCommand> Orders { get; set; }
    }

    public class CreateOrderCommand
    {
        public DateTime OrderDate { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string OrderStatus { get; set; } = "Processing";
        public List<CreateOrderEventsCommand> Events { get; set; }
    }

    public class CreateOrderEventsCommand
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EventType { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public double Amount { get; set; }
    }

    public class CreateBookingAndOrdersHandler(IBookingService bookingService) : IRequestHandler<CreateBookingAndOrdersCommand, Result>
    {
        public async Task<Result> Handle(CreateBookingAndOrdersCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var bookingId = await bookingService.CreateBookingAndOrdersAsync(command, cancellationToken);

                return Result.Success(bookingId);
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("UnexpectedError", ex.Message, (int)HttpStatusCode.InternalServerError));
            }
        }
    }

    public class CreateBookingEndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapPost("api/booking-orders/create", async (CreateBookingAndOrdersCommand command, IMediator mediator) =>
            {
                return Results.Ok(await mediator.Send(command));
            }).RequireAuthorization();
        }
    }
}
