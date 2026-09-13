using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.Outbox;
using BuildingBlocks.Messaging.Pulsar;
using DotPulsar;
using DotPulsar.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyStoreProject.Services.Inventory.Application.Abstractions.Data;
using MyStoreProject.Services.Inventory.Infrastructure.BackgroundJobs;
using MyStoreProject.Services.Inventory.Infrastructure.Persistence;

namespace MyStoreProject.Services.Inventory.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetConnectionString("InventoryConnectionString")
                ?? throw new InvalidOperationException("Connection string not found");
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings)));
            
            var pulsarUrl = configuration["Pulsar:ServiceUrl"] 
                            ?? throw new InvalidOperationException("Pulsar Url not found");

            services.AddSingleton< IPulsarClient>(_ => PulsarClient.Builder()
                .ServiceUrl(new Uri(pulsarUrl))
                .Build());

            services.AddSingleton<IEventPublisher, PulsarEventPublisher>();
            
            services.AddHostedService<OutboxProcessorJob<InventoryDbContext>>();
            services.AddHostedService<OrderPlacedConsumerJob>();
            services.AddHostedService<PaymentFailedConsumerJob>();
            services.AddHostedService<PaymentSuccesConsumerJob>();
            services.AddScoped<IInventoryDbContext>(sp => sp.GetRequiredService<InventoryDbContext>());
            return services;
        }
    }
}