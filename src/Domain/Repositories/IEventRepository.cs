using HumanResources.Domain.Entities;

namespace HumanResources.Domain.Repositories;

public interface IEventRepository
{
    Task<Event> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Event evt, CancellationToken cancellationToken);
    Task UpdateAsync(Event evt, CancellationToken cancellationToken);
}