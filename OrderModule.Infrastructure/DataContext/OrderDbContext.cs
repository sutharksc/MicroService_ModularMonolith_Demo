using Microsoft.EntityFrameworkCore;
using OrderModule.Domain.Models;

namespace OrderModule.Infrastructure.DataContext
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderEvents> OrderEvents { get; set; }
    }
}
