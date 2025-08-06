using BookingModule.Application.Features.Booking;
using SharedModule.Booking.Grpc;

namespace BookingModule.Application.Abstractions
{
    public interface IBookingService
    {
        Task<Guid> CreateBookingAndOrdersAsync(CreateBookingAndOrdersCommand command, CancellationToken cancellationToken);
        Task<BookingResponse?> GetBookingByIdAsync(string bookingId);
    }
}
