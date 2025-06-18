using HumanResources.Application.Commands;
using HumanResources.Application.DTOs;
using HumanResources.Domain.Repositories;
using HumanResources.Domain.Services;
using HumanResources.Domain.Entities;
using HumanResources.Domain.Events;

namespace HumanResources.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IExternalService _externalService;

    private readonly ISenderService _senderService;

    public EventService(IEventRepository eventRepository, IExternalService externalService, ISenderService senderService)
    {
        _eventRepository = eventRepository;
        _externalService = externalService;
        _senderService = senderService;
    }

    public async Task<EventDto> AddEventAsync(AddEventCommand command, CancellationToken cancellationToken)
    {
        var evt = Event.Create(command.Name, command.StartDate, command.EndDate);
        await _eventRepository.AddAsync(evt, cancellationToken);

        // TODO: Implement outbox pattern by persisting domain events to Cosmos DB
        foreach (var domainEvent in evt.DomainEvents)
        {
            await _senderService.PublishAsync<DomainEvent>(domainEvent);
        }
        evt.ClearDomainEvents();

        return new EventDto(evt.Id, evt.Name, evt.StartDate, evt.EndDate, evt.Status.ToString());
    }

    public async Task ExpireEventAsync(ExpireEventCommand command, CancellationToken cancellationToken)
    {
        var evt = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken);
        if (evt == null)
            throw new InvalidOperationException("Event not found.");

        evt.Expire();
        await _eventRepository.UpdateAsync(evt, cancellationToken);

        // Only publish domain events if needed for the template
        foreach (var domainEvent in evt.DomainEvents)
        {
            // Placeholder: Publish to messaging system
        }
        evt.ClearDomainEvents();
    }

    public async Task CloseEventAsync(CloseEventCommand command, CancellationToken cancellationToken)
    {
        var evt = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken);
        if (evt == null)
            throw new InvalidOperationException("Event not found.");

        evt.Close();
        await _eventRepository.UpdateAsync(evt, cancellationToken);

        // Only publish domain events if needed for the template
        foreach (var domainEvent in evt.DomainEvents)
        {
            // Placeholder: Publish to messaging system
        }
        evt.ClearDomainEvents();
    }
}
