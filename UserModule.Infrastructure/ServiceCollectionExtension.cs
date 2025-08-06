using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserModule.Application;
using UserModule.Application.Abstractions;
using UserModule.Domain.Models;
using UserModule.Infrastructure.DataContext;
using UserModule.Infrastructure.gRPC;
using UserModule.Infrastructure.Services;

namespace UserModule.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUserModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();

            services.AddScoped<IUserService, UserService>();

            services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("UserConn")));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Tokens.AuthenticatorTokenProvider = "email";
            })
           .AddEntityFrameworkStores<UserDbContext>()
           .AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                dbContext.Database.Migrate();
            }

            return services;
        }

        public static IApplicationBuilder UseUserModule(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserGrpcService>();
            });

            return app;
        }
    }
}
