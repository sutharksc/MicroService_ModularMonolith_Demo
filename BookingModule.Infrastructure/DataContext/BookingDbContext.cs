using BookingModule.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingModule.Infrastructure.DataContext
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }
        public DbSet<Booking> Bookings { get; set; }
    }
}
