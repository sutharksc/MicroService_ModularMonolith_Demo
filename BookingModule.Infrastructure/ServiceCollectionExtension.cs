using BookingModule.Application;
using BookingModule.Application.Abstractions;
using BookingModule.Infrastructure.DataContext;
using BookingModule.Infrastructure.gRPC;
using BookingModule.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedModule.Protos;
using SharedModule.Shared;

namespace BookingModule.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBookingModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddScoped<IBookingService, BookingService>();

            services.AddGrpcClient<OrderService.OrderServiceClient>(options =>
            {
                options.Address = new Uri(AppConsts.OrderGrpcServiceUrl);
            });

            services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BookingConn")));

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
                dbContext.Database.Migrate();
            }
            return services;
        }

        public static IApplicationBuilder UseBookingModule(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<BookingGrpcService>();
            });

            return app;
        }
    }
}
