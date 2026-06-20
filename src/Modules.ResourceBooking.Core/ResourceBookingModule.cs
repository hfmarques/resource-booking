using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.API;

namespace Modules.ResourceBooking.Core;

public static class ResourceBookingModule
{
    public static IServiceCollection AddResourceBookingModule(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(ResourceBookingModule).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.RegisterHandlersFromAssembly(assembly);
        services.RegisterApiEndpointsFromAssembly(assembly);

        return services;
    }
}
