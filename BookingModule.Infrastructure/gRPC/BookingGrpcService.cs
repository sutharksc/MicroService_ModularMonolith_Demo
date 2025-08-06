using BookingModule.Application.Abstractions;
using Grpc.Core;
using SharedModule.Booking.Grpc;

namespace BookingModule.Infrastructure.gRPC
{
    public class BookingGrpcService(IBookingService bookingService) : BookingService.BookingServiceBase
    {
        public override async Task<BookingResponse?> GetBookingById(GetBookingByIdRequest request, ServerCallContext context)
        {
            var booking = await bookingService.GetBookingByIdAsync(request.Id);

            return booking;
        }
    }
}
