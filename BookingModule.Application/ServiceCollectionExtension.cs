using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookingModule.Application
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            //services.AddCQRS(cfg =>
            //{
            //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //});

            MediatRServiceConfiguration configuration = new();
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(configuration);
        }
    }

    public class BookingModuleIdentityRoot { }


}
