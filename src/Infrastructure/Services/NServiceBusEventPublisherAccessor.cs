    using HumanResources.Application.Services;

namespace HumanResources.Infrastructure.Services
{
    public static class NServiceBusEventPublisherAccessor
    {
        public static ISenderService? Instance { get; set; }
    }
}
