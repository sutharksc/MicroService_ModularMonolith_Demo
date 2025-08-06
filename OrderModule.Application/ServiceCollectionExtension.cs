using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OrderModule.Application
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            MediatRServiceConfiguration configuration = new();
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(configuration);
        }
    }

    public class OrderModuleIdentityRoot { }
}
