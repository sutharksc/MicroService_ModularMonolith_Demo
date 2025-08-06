using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedModule.EndPoint
{
    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder builder);
    }

    public static class MinimalApiExtension
    {
        public static IEndpointRouteBuilder MapMinimalEndpoints(this IEndpointRouteBuilder builder)
        {
            var scope = builder.ServiceProvider.CreateScope();

            var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();

            foreach (var endpoint in endpoints)
            {
                Console.WriteLine($" - {endpoint.GetType().FullName}");
                endpoint.MapEndpoint(builder);
            }

            return builder;
        }

        public static IServiceCollection RegisterEndpointsFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<IEndpoint>())
                .As<IEndpoint>()
                .WithTransientLifetime());

            return services;
        }
    }
}
