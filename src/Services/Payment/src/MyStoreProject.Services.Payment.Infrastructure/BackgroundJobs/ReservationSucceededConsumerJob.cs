using BuildingBlocks.Messaging.Inbox;
using DotPulsar.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyStoreProject.Contracts.Common;
using MyStoreProject.Contracts.Events;
using MyStoreProject.Services.Payment.Application.Payment.Commands.ProcessPayment;
using MyStoreProject.Services.Payment.Infrastructure.Persistence;

namespace MyStoreProject.Services.Payment.Infrastructure.BackgroundJobs;

public class ReservationSucceededConsumerJob : BaseEventConsumer<PaymentDbContext, ReservationSucceeded>
{
    private readonly IServiceProvider _serviceProvider;
    public ReservationSucceededConsumerJob(
        IServiceProvider serviceProvider,
        IPulsarClient pulsarClient,
        ILogger<ReservationSucceededConsumerJob> logger)
    : base(serviceProvider, pulsarClient, logger, PulsarTopics.ReservationSucceeded, PulsarSubscriptions.PaymentsReservationSucceeded)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<bool> HandleEventAsync(ReservationSucceeded @event, PaymentDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        var reservationSuccessCommand = new ProcessPaymentCommand(
            @event.EventId,
            @event.OrderId,
            @event.Items.Sum(i => i.Quantity * i.Price));

        var result = await sender.Send(reservationSuccessCommand, cancellationToken);
       
        return result.IsSuccess;
    }
}