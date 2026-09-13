using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Outbox;
using BuildingBlocks.Messaging.Pulsar;
using DotPulsar;
using DotPulsar.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyStoreProject.Services.Payment.Application.Abstractions.Data;
using MyStoreProject.Services.Payment.Infrastructure.BackgroundJobs;
using MyStoreProject.Services.Payment.Infrastructure.Persistence;

namespace MyStoreProject.Services.Payment.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetConnectionString("PaymentConnectionsString")
                ?? throw new InvalidOperationException("Connection string is null.");

            
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings)));
            services.AddScoped<IPaymentDbContext>(sp => sp.GetRequiredService<PaymentDbContext>());

            var pulsarUrl = configuration["Pulsar:ServiceUrl"] 
                            ?? throw new InvalidOperationException("Pulsar Url not found");
            
            services.AddSingleton<IPulsarClient>(_ => PulsarClient.Builder()
                .ServiceUrl(new Uri(pulsarUrl))
                .Build());

            services.AddHostedService<OutboxProcessorJob<PaymentDbContext>>();
            services.AddHostedService<ReservationSucceededConsumerJob>();

            services.AddSingleton<IEventPublisher, PulsarEventPublisher>();
            return services;
        }
    }
}