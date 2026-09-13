using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

using MyStoreProject.Services.Ordering.Application.Behaviors;

namespace MyStoreProject.Services.Ordering.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
  
}