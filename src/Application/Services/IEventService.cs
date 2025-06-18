using HumanResources.Application.Commands;
using HumanResources.Application.DTOs;

namespace HumanResources.Application.Services;

public interface IEventService
{
    Task<EventDto> AddEventAsync(AddEventCommand command, CancellationToken cancellationToken);
    Task ExpireEventAsync(ExpireEventCommand command, CancellationToken cancellationToken);
    Task CloseEventAsync(CloseEventCommand command, CancellationToken cancellationToken);
}
