using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderModule.Application;
using OrderModule.Application.Abstractions;
using OrderModule.Infrastructure.DataContext;
using OrderModule.Infrastructure.gRPC;
using OrderModule.Infrastructure.Services;

namespace OrderModule.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOrderModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();

            services.AddScoped<IOrderService, OrderService>();

            services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderConn")));

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                dbContext.Database.Migrate();
            }

            return services;
        }

        public static IApplicationBuilder UseOrderModule(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<OrderGrpcService>();
            });

            return app;
        }
    }
}
