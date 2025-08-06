using BookingModule.Application.Abstractions;
using BookingModule.Application.Features.Booking;
using BookingModule.Domain.Models;
using BookingModule.Infrastructure.DataContext;
using Grpc.Net.Client;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedModule.Abstractions;
using SharedModule.Booking.Grpc;
using SharedModule.Protos;
using SharedModule.Shared;

namespace BookingModule.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingDbContext bookingDbContext;
        private readonly OrderService.OrderServiceClient orderClient;
        private readonly IUserContext userContext;
        public BookingService(BookingDbContext bookingDbContext, IUserContext userContext)
        {
            this.bookingDbContext = bookingDbContext;
            this.userContext = userContext;
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var orderServiceUrl = AppConsts.OrderGrpcServiceUrl;
            var channel = GrpcChannel.ForAddress(orderServiceUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            orderClient = new OrderService.OrderServiceClient(channel);
        }
        public async Task<Guid> CreateBookingAndOrdersAsync(CreateBookingAndOrdersCommand command, CancellationToken cancellationToken)
        {
            var booking = command.Adapt<Booking>();
            booking.UserId = userContext.UserId;
            booking.Id = Guid.NewGuid();
            booking.CreatedAt = DateTime.Now;
            booking.ModifiedAt = DateTime.Now;

            await bookingDbContext.AddAsync(booking, cancellationToken);
            await bookingDbContext.SaveChangesAsync(cancellationToken);

            foreach (var orderCommand in command.Orders)
            {

                var orderRequest = new CreateOrderRequest
                {
                    BookingId = booking.Id.ToString(),
                    UserId = userContext.UserId.ToString(),
                    OrderDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    OrderTotal = Convert.ToDouble(orderCommand.Events.Sum(x => x.Amount)),
                    PaymentStatus = orderCommand.PaymentStatus,
                    OrderStatus = orderCommand.OrderStatus,
                };

                orderRequest.Events.AddRange(orderCommand.Events.Select(x => new OrderEventRequest
                {
                    StartDate = x.StartDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    Amount = x.Amount,
                    EventType = x.EventType,
                    Name = x.Name,
                    Venue = x.Venue,
                }));


                await orderClient.CreateOrderAsync(orderRequest);
            }

            return booking.Id;
        }

        public async Task<BookingResponse?> GetBookingByIdAsync(string bookingId)
        {
            var booking = await bookingDbContext.Bookings.FirstOrDefaultAsync(x => x.Id == Guid.Parse(bookingId));

            if (booking == null) return null;

            return booking.Adapt<BookingResponse?>();
        }
    }
}
