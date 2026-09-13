using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Outbox;
using BuildingBlocks.Messaging.Pulsar;
using DotPulsar;
using DotPulsar.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyStoreProject.Services.Ordering.Application.Abstractions.Data;
using MyStoreProject.Services.Ordering.Infrastructure.BackgroundJobs;
using MyStoreProject.Services.Ordering.Infrastructure.Persistence;

namespace MyStoreProject.Services.Ordering.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("OrderingConnectionString")
                                   ?? throw new InvalidOperationException("Connection string not found");
            services.AddDbContext<OrderDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            
            var pulsarUrl = configuration["Pulsar:ServiceUrl"] 
                ?? throw new InvalidOperationException("Pulsar Url not found");
            
            services.AddSingleton<IPulsarClient>(_ => PulsarClient.Builder()
                .ServiceUrl(new Uri(pulsarUrl))
                .Build());

            services.AddHostedService<OutboxProcessorJob<OrderDbContext>>();

            services.AddSingleton<IEventPublisher, PulsarEventPublisher>();
            services.AddHostedService<ReservationFailedConsumerJob>();
            services.AddHostedService<ReservationSucceededConsumerJob>();
            services.AddHostedService<PaymentFailedConsumerJob>();
            services.AddHostedService<PaymentSuccessConsumerJob>();
            services.AddScoped<IOrderDbContext>(sp => sp.GetRequiredService<OrderDbContext>());
            
            return services;
        }

    }
}