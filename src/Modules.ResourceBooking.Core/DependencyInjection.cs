using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Domain;

namespace Modules.ResourceBooking.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddResourceBookingCore(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        var handlerTypes = assembly.GetTypes()
            .Where(t => typeof(IHandler).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var handlerType in handlerTypes)
        {
            services.AddScoped(handlerType);
        }

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
