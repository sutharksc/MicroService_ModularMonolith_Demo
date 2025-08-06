using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using OrderModule.Application.Abstractions;
using OrderModule.Application.Features.Order;
using OrderModule.Domain.Models;
using OrderModule.Infrastructure.DataContext;
using SharedModule.Abstractions;
using SharedModule.Booking.Grpc;
using SharedModule.Protos;
using SharedModule.Shared;

namespace OrderModule.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext orderDbContext;
        private readonly BookingService.BookingServiceClient bookingClient;
        private readonly UserService.UserServiceClient userClient;
        private readonly IUserContext userContext;
        public OrderService(OrderDbContext orderDbContext, IUserContext userContext)
        {
            this.orderDbContext = orderDbContext;
            this.userContext = userContext;
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var bookingServiceUrl = AppConsts.BookingGrpcServiceUrl;
            var bookingChannel = GrpcChannel.ForAddress(bookingServiceUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            bookingClient = new BookingService.BookingServiceClient(bookingChannel);

            var userServiceUrl = AppConsts.UserGrpcServiceUrl;
            var userChannel = GrpcChannel.ForAddress(userServiceUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            userClient = new UserService.UserServiceClient(userChannel);
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderRequest request)
        {
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                BookingId = Guid.Parse(request.BookingId),
                UserId = Guid.Parse(request.UserId),
                OrderDate = DateTime.Parse(request.OrderDate),
                OrderTotal = Convert.ToDecimal(request.OrderTotal),
                PaymentStatus = request.PaymentStatus,
                OrderStatus = request.OrderStatus,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Events = request.Events.Select(i => new OrderEvents
                {
                    Id = Guid.NewGuid(),
                    Name = i.Name,
                    StartDate = DateTime.Parse(i.StartDate),
                    EndDate = DateTime.Parse(i.EndDate),
                    OrderId = orderId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    EventType = i.EventType,
                    Amount = Convert.ToDecimal(i.Amount),
                    Venue = i.Venue
                }).ToList()
            };

            await orderDbContext.Orders.AddAsync(order);
            await orderDbContext.SaveChangesAsync();

            return order.Id;
        }

        public async Task<GetOrderByIdResult?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = await orderDbContext.Orders
                        .Include(x => x.Events)
                        .Where(x => x.Id == id)
                        .Select(x => new GetOrderByIdResult
                        {
                            Id = id,
                            BookingId = x.BookingId,
                            OrderDate = x.OrderDate,
                            UserId = x.UserId,
                            OrderStatus = x.OrderStatus,
                            OrderTotal = x.OrderTotal,
                            PaymentStatus = x.PaymentStatus,
                            Events = x.Events.Select(y => new OrderEventResult
                            {
                                Id = y.Id,
                                OrderId = y.OrderId,
                                StartDate = y.StartDate,
                                EndDate = y.EndDate,
                                Name = y.Name,
                                EventType = y.EventType,
                                Venue = y.Venue,
                                Amount = y.Amount
                            }).ToList()
                        }).SingleOrDefaultAsync();

            if (order == null) return null;

            var booking = await bookingClient.GetBookingByIdAsync(new GetBookingByIdRequest { Id = order.BookingId.ToString() });
            var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { UserId = order.UserId.ToString() });

            order.Booking = booking;
            order.User = user;

            return order;
        }

    }
}
